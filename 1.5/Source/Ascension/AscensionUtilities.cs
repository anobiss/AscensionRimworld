﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using static HarmonyLib.Code;
using Random = System.Random;

namespace Ascension
{
    public class AscensionUtilities
    {

        public static float GetQualityMultiplier(int quality)
        {
            if (quality == 0)
            {
                return 0.5f;
            }
            else if (quality == 1)
            {
                return 1f;
            }
            else if (quality > 1 && quality < 6)
            {
                return quality;
            }else if (quality == 6)
            {
                return 10f;
            }
            return 1f;
        }

        public static readonly float[] spiritPillOffsetRates = {5f, 7f, 10f, 12f, 17f, 20f};
        public static readonly float[] spiritPillCostRates = { 12000f, 77000f, 100000f, 120000f, 200000f, 1000000f, 12000000f };//how much qi each tier costs	Poor,Normal,Good,Excellent,Masterwork,Legendary
    //essence realm max qi rates
    public static readonly float[] maxQiRates = { 2f, 10f, 100f, 500f, 1000f, 10000f, 120000f };
        public static void UpdateRealmMaxQi(int index, QiPool_Hediff qiPool)
        {

            qiPool.realmMaxAmountOffset = maxQiRates[index];
        }

        public static bool Can
            
            (Realm_Hediff hediff)
        {
            if (hediff.progress == 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanControl(Pawn SelPawn)
        {
            Pawn selPawnForGear = SelPawn;
            if (selPawnForGear.Dead || selPawnForGear == null)
            {
                return false;
            }
            if (selPawnForGear.Downed || selPawnForGear.InMentalState)
            {
                return false;
            }
            if (selPawnForGear.Faction != Faction.OfPlayer && !selPawnForGear.IsPrisonerOfColony)
            {
                return false;
            }
            if (selPawnForGear.IsPrisonerOfColony && selPawnForGear.Spawned && !selPawnForGear.Map.mapPawns.AnyFreeColonistSpawned)
            {
                return false;
            }
            if (selPawnForGear.IsPrisonerOfColony && (PrisonBreakUtility.IsPrisonBreaking(selPawnForGear) || (selPawnForGear.CurJob != null && selPawnForGear.CurJob.exitMapOnArrival)))
            {
                return false;
            }
            return true;
        }

        public int damageAmountBase = -1;

        public float armorPenetrationBase = -1f;

        public int postExplosionSpawnThingCount = 1;

        public int preExplosionSpawnThingCount = 1;

        public bool doVisualEffects = true;

        public float propagationSpeed = 1f;

        public static void GiveCultivator(Pawn pawn)
        {
            if (!pawn.health.hediffSet.HasHediff(AscensionDefOf.Cultivator))
            {
                pawn.health.AddHediff(AscensionDefOf.Cultivator);
                if (!pawn.health.hediffSet.HasHediff(AscensionDefOf.BodyRealm) && !pawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm))
                {
                    pawn.health.AddHediff(AscensionDefOf.BodyRealm).Severity = 1;
                }
            }
        }

        public static void GoldenCoreBreakthrough(Pawn pawn, int score)
        {
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                cultivatorHediff.goldenCoreScore = score;
                Log.Message("score" + score);
            }
            AscensionUtilities.TierBreakthrough((Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm));
        }
        public static float UpdateCultivationSpeed(Cultivator_Hediff cultivatorHediff)//we do this before starting jobs to update the cultivation speed, we return a float to make sure we have cultivation speed after update, we also take in optional cords if they have a cultivation spot to walk to
        {
            float cultivationSpeed = 0;//if cult speed is 0 we know we messed something up. 
            QiGatherMapComponent qiGatherMapComp = cultivatorHediff.pawn.Map.GetComponent<QiGatherMapComponent>();
            ElementEmitMapComponent elementEmitMapComp = cultivatorHediff.pawn.Map.GetComponent<ElementEmitMapComponent>();
            if (cultivatorHediff != null)
            {
                //we do these speed calcs in all cultivation realms                  here we do the base times the speedoffset 
                cultivationSpeed = (cultivatorHediff.cultivationBaseSpeed * cultivatorHediff.cultivationSpeedOffset);
                if (cultivatorHediff.pawn.needs.mood != null)
                {
                    //then times it by our mood but plus some so they can cultivate when upset
                    cultivationSpeed *= (0.4f + cultivatorHediff.pawn.needs.mood.CurLevelPercentage);
                }
                //check if they are in essence realm, then check for nearby gather qi things.
                if (cultivatorHediff.pawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm) && qiGatherMapComp != null)
                {
                    //we increase the speed here by the amount of gather qi in the tile divided by 100
                    int qiTile = qiGatherMapComp.GetQiGatherAt(cultivatorHediff.pawn.Position.x, cultivatorHediff.pawn.Position.z);
                    //Log.Message("qi at position is" + qiTile);
                    cultivationSpeed *= (1 + qiTile / 100);//its 1 plus 1% qitile
                    //Log.Message("essence realm cultivation speed is" + cultivationSpeed);
                }
                if (elementEmitMapComp != null)
                {
                    cultivationSpeed *= 1 + (elementEmitMapComp.CalculateElementValueAt(new IntVec2(cultivatorHediff.pawn.Position.x, cultivatorHediff.pawn.Position.z), cultivatorHediff.element) / 100f);
                }
                cultivatorHediff.cultivationSpeed = cultivationSpeed;
            }
            if (cultivationSpeed < 0.1f)//slowest is 0.1.
            {
                cultivationSpeed = 0.1f;
            }
            return cultivationSpeed;
        }

        public static void UpdateQiMax(QiPool_Hediff hediff)
        {
            //body size times 100, then realm and (if above 0) gc scores are added, then offset is applied, then inner cauldron is added
            Cultivator_Hediff cultivatorHediff = hediff.pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;

            //make sure the max qi factors display reflects how this works in a understandable way for the player.
            hediff.maxAmount = (long)Math.Floor((hediff.pawn.RaceProps.baseBodySize * 100f));

            //
            if (hediff.realmMaxAmountOffset > 0)
            {
                hediff.maxAmount = hediff.maxAmount * hediff.realmMaxAmountOffset;
            }

            if (cultivatorHediff != null)
            {
                if (cultivatorHediff.goldenCoreScore > 0)
                {
                    hediff.maxAmount += cultivatorHediff.goldenCoreScore;
                }
                hediff.maxAmount *= hediff.maxAmountOffset;
                hediff.maxAmount += cultivatorHediff.innerCauldronQi;
            }

            if (hediff.amount > hediff.maxAmount)//so that if your max qi is reduced you dont have more than max
            {
                hediff.amount = hediff.maxAmount;
            }
        }
        public static void IncreaseQi(Pawn pawn, float amount, bool noExplosion = false)
        {
            HediffSet hediffSet = pawn.health.hediffSet;
            if (!hediffSet.HasHediff(AscensionDefOf.QiPool))
            { //gives hediffs that r missing.
                if (!hediffSet.HasHediff(AscensionDefOf.Cultivator))
                {
                    GiveCultivator(pawn);
                }
                pawn.health.AddHediff(AscensionDefOf.QiPool);
            }
            QiPool_Hediff qiHediff = hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;

            float newQiAmount = qiHediff.amount + amount;
            if (newQiAmount > qiHediff.maxAmount)//checks if over max then blows them up and sets newqiamount to max amount
            {
                if (!noExplosion)
                {
                    float explosionRadius = (newQiAmount - qiHediff.maxAmount) / 200;
                    if (explosionRadius > 56.4)//true max is 56.4
                    {
                        explosionRadius = 56.4f;
                    }
                    GenExplosion.DoExplosion(pawn.PositionHeld, pawn.MapHeld, explosionRadius, DamageDefOf.Bomb, pawn, -1, -1, null, null, null, null, null, 0, 0, null, false, null, 0, 0, 0, false, null, null, null, true, 1, 0f, true, null, 1f);
                }
                qiHediff.amount = qiHediff.maxAmount;
            }else
            {
                qiHediff.amount = newQiAmount;
            }

        }

        //used in tier progress method to make progress not go above 100
        private static void ProgressTier(Realm_Hediff hediff, float progress)
        {
            //if they are at 100% we shouldnt add any severity and let breakthroughs do that.

            //calculate the percentage after progress for maths
            //since we give ascendant foundations from the parent method, we should use it to make it start at 1 severity. that way we can avoid complicated maths

            
            //if its less than one we need to correct it

            if (hediff.Severity < 1)
            {
                hediff.Severity = (1);

            }
            if (hediff.progress + progress < hediff.maxProgress)
            {
                hediff.progress += progress;
                Log.Message("increased realm progress by "+progress);
            }
            else
            {
                hediff.progress = hediff.maxProgress;
                if (hediff.def != AscensionDefOf.EssenceRealm)
                {
                    if (PawnUtility.ShouldSendNotificationAbout(hediff.pawn))
                    {
                        Find.LetterStack.ReceiveLetter("AS_CanBreakThrough".Translate(), "AS_CanBreakThroughDesc".Translate(hediff.pawn.NameFullColored.Named("PAWN"), hediff.CurStage.label.Named("REALM")), AscensionDefOf.AS_CultivationBreakthroughMessage, hediff.pawn);
                    }
                }else if (hediff.Severity < 7)
                {
                    if (PawnUtility.ShouldSendNotificationAbout(hediff.pawn))
                    {
                        Find.LetterStack.ReceiveLetter("AS_CanBreakThrough".Translate(), "AS_CanBreakThroughDesc".Translate(hediff.pawn.NameFullColored.Named("PAWN"), hediff.CurStage.label.Named("REALM")), AscensionDefOf.AS_CultivationBreakthroughMessage, hediff.pawn);
                    }
                }

            }
        }

        //We use this to increase tiers to prevent them from advancing a tier without a breakthrough.


        //only used in tribulation for essence realms, and exercise in body realms
        //this part checks if they even have the hediff and if not gives it to them since the cultivator hediff should've done so. first stages have no buffs so are fine to give for free
        public static void TierProgress(Pawn pawn,HediffDef hediffDef, float progress)
        {
            //checks if they have the cultivator hediff and if not gives it
            if (!pawn.health.hediffSet.HasHediff(AscensionDefOf.Cultivator))
            {
                pawn.health.AddHediff(AscensionDefOf.Cultivator);
            }
            if (!pawn.health.hediffSet.HasHediff(AscensionDefOf.QiPool))
            {
                pawn.health.AddHediff(AscensionDefOf.QiPool);
            }

            if (hediffDef == AscensionDefOf.BodyRealm)
            {
                if (!pawn.health.hediffSet.HasHediff(AscensionDefOf.BodyRealm))
                {
                    pawn.health.AddHediff(AscensionDefOf.BodyRealm).Severity = 1;
                }
            }
            Realm_Hediff hediff = (Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef, false);
            ProgressTier(hediff, progress);
        }

        public static void CauldronIncrease(Pawn pawn, float amount)
        {
            //checks if they have the cultivator hediff and if not gives it
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator, false) as Cultivator_Hediff;
            if (cultivatorHediff == null)
            {
                pawn.health.AddHediff(AscensionDefOf.Cultivator);
            }
            Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm, false) as Realm_Hediff;
            if (essenceHediff != null)
            {
                if (essenceHediff.Severity < 3)
                {
                    if ((cultivatorHediff.innerCauldronQi + amount) < cultivatorHediff.innerCauldronLimit)
                    {
                        cultivatorHediff.innerCauldronQi += amount;
                    }else if ((cultivatorHediff.innerCauldronQi + amount) > cultivatorHediff.innerCauldronLimit)
                    {
                        cultivatorHediff.innerCauldronQi = cultivatorHediff.innerCauldronLimit;
                    }
                }else
                {
                    cultivatorHediff.innerCauldronQi += amount;// if the hediff isnt null and isnt less than 3 we know they have a golden core and therefore cap should be removed
                }
            }else
            {
                if (cultivatorHediff.innerCauldronQi + amount < cultivatorHediff.innerCauldronLimit)
                {
                    cultivatorHediff.innerCauldronQi += amount;
                }
            }
            UpdateQiMax(pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff);
        }


        public static HediffDef RandomAbilityScroll(HashSet<HediffDef> alreadyAdded, Pawn pawn)//used with random ability chance setting in harmony addiction generator patch to select random ability fo pawns
        {
            Realm_Hediff essenceRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
            Realm_Hediff bodyRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
            List<HediffDef> scrollList = new(AscensionStaticStartUtils.ScrollHediffList);
            HediffDef randomScroll;
            Scroll_Hediff randomScrollHediff;









            bool reqRealmFlag = false;
            // Keep selecting a random scroll until one is found that is not already in alreadyAdded
            do
            {
                randomScroll = scrollList.RandomElement();
                randomScrollHediff = HediffMaker.MakeHediff(randomScroll, pawn) as Scroll_Hediff;
                HediffComp_AddScrollAbility scrollAbilityComp = randomScrollHediff.TryGetComp<HediffComp_AddScrollAbility>();
                if (scrollAbilityComp.Props.reqEssence > (int)Math.Floor(essenceRealm.Severity) || scrollAbilityComp.Props.reqBody > (int)Math.Floor(bodyRealm.Severity))//checks if pawn has the realm requirements
                {
                    reqRealmFlag = true;
                }
                else { 
                }
            }
            while (alreadyAdded.Contains(randomScroll) || reqRealmFlag == true);// or while required realm tier is higher
            //if it runs out of scrolls it goes on forever!!!!!


            if (randomScroll == null)
            {
                Log.Message("null random scroll");
            }

            return randomScroll;
        }


        //remember we are giving those who get any cultivation stuff 
        //for the first time the hidden cultivator hediff


        //public static void AddRandomCultivationRealm(Pawn pawn, string realmType, int stage)
        //{
        //    Realm_Hediff randRealm;
        //    if (realmType == "Essence")
        //    {
        //        //remember arrays start at 0
        //        pawn.health.AddHediff(essenceRealms[stage + 1]);
        //    }
        //    else if (realmType == "Body")
        //    {
        //        pawn.health.AddHediff(bodyRealms[stage + 1]);
        //    }
        //    else
        //    {
        //        Log.Message("Error no realm type of that name found");
        //    }
        //}




        //method to get the current stage of a given realm


        //you should never have two of the same realms, especially when calling this

        //finds the current realm hediff of a given realm
        //public static Realm_Hediff FindCurrentRealm(Pawn pawn, string realmType)
        //{
        //    //for each loop checking every hediff on pawn to find the realm
        //    Realm_Hediff hediff_Realm = null;
        //    List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
        //    for (int i = 0; i < hediffs.Count; i++)
        //    {
        //        //check if its a realm and is the type we want
        //        if (hediffs[i] is Realm_Hediff hediff_Realm2 && (hediff_Realm.type == realmType))
        //        {
        //            hediff_Realm = hediff_Realm2;
        //        }
        //    }
        //    return hediff_Realm;
        //}
        public static readonly int[] maxProgressionRatesBody = { 100, 200, 420, 500, 700, 1200, 2400}; // max qi offset to set to when advancing. first is tier 2
        public static readonly int[] maxProgressionRatesEssence = { 7000, 120000, 700000, 7000000, 12000000, 70000000, 120000000 }; // max qi offset to set to when advancing. first is tier 2
        public static void UpdateMaxProg(Realm_Hediff realmHediff)
        {
            int tier = ((int)Math.Floor(realmHediff.Severity));
            if (realmHediff.def == AscensionDefOf.EssenceRealm)
            {
                if (tier <= 7)
                {
                    if (tier >= 2)
                    {
                        int progMax = maxProgressionRatesEssence[tier - 1];
                        realmHediff.maxProgress = progMax;
                    }else
                    {
                        realmHediff.maxProgress = 100;
                    }
                }

            }else
            {
                if (tier >= 2 && tier <= 5)
                {
                    int progMax = maxProgressionRatesBody[tier - 1];
                    realmHediff.maxProgress = progMax;
                }
            }

        }

        //Golden Core breakthroughs work entirely different from every other.
        //100 max qi per (hour divided by cultivation speed)
        //finishes when interupted or ended by running out of qi
        //qi recovery is turned off during this breakthrough

        public static void GoldenCoreBreakthrough(Realm_Hediff realmHediff)
        {
            if (realmHediff != null)
            {
                if (realmHediff.progress >= realmHediff.maxProgress)
                {
                    int maxSeverityCap = 7;
                    if (realmHediff.def == AscensionDefOf.EssenceRealm)
                    {
                        maxSeverityCap = 7;
                    }

                    UpdateMaxProg(realmHediff); //always update max prog on breakthroughs and when the hediff is added.
                    if (realmHediff.Severity < 2)//if they stage one breakthrough to stage two
                    {
                        realmHediff.Severity = 2;
                        realmHediff.progress = 0;
                        if (PawnUtility.ShouldSendNotificationAbout(realmHediff.pawn))
                        {
                            Find.LetterStack.ReceiveLetter(realmHediff.Label + " " + "AS_Breakthrough".Translate(), realmHediff.pawn.NameFullColored + " " + realmHediff.CurStage.extraTooltip, AscensionDefOf.AS_CultivationBreakthroughMessage, realmHediff.pawn);
                        }
                    }
                    else if (realmHediff.Severity < maxSeverityCap) // this is okay right now because both realms have only 5
                    {
                        realmHediff.Severity += 1;
                        realmHediff.progress = 0;
                        if (PawnUtility.ShouldSendNotificationAbout(realmHediff.pawn))
                        {
                            Find.LetterStack.ReceiveLetter(realmHediff.Label + " " + "AS_Breakthrough".Translate(), realmHediff.pawn.NameFullColored + " " + realmHediff.CurStage.extraTooltip, AscensionDefOf.AS_CultivationBreakthroughMessage, realmHediff.pawn);
                        }
                    }
                    else if (realmHediff.Severity >= maxSeverityCap && realmHediff.def == AscensionDefOf.BodyRealm)// if its maxcap and its body we move onto essence realms
                    {
                        Pawn pawn = realmHediff.pawn;
                        pawn.health.AddHediff(AscensionDefOf.EssenceRealm).Severity = 1;
                        pawn.health.RemoveHediff(realmHediff);

                    }
                }
            }
        }


        //We use this to breakthrough to the next tier if progression is at 100% of the current tier, psuedo-immortality takes ascension to do this.
        public static void TierBreakthrough(Realm_Hediff realmHediff)
        {
            if (realmHediff != null)
            {
                if (realmHediff.progress >= realmHediff.maxProgress)
                {
                    int maxSeverityCap = 7;
                    if (realmHediff.def == AscensionDefOf.EssenceRealm)
                    {
                        maxSeverityCap = 7;
                    }
                    
                    UpdateMaxProg(realmHediff); //always update max prog on breakthroughs and when the hediff is added.
                    if (realmHediff.Severity < 2)//if they stage one breakthrough to stage two
                    {
                        realmHediff.Severity = 2;
                        realmHediff.progress = 0;
                        if (PawnUtility.ShouldSendNotificationAbout(realmHediff.pawn))
                        {
                            Find.LetterStack.ReceiveLetter(realmHediff.Label+" "+"AS_Breakthrough".Translate(), realmHediff.pawn.NameFullColored+" "+realmHediff.CurStage.extraTooltip, AscensionDefOf.AS_CultivationBreakthroughMessage, realmHediff.pawn);
                        }
                    } else if (realmHediff.Severity < maxSeverityCap) // this is okay right now because both realms have only 5
                    {
                        realmHediff.Severity += 1;
                        realmHediff.progress = 0;
                        if (PawnUtility.ShouldSendNotificationAbout(realmHediff.pawn))
                        {
                            Find.LetterStack.ReceiveLetter(realmHediff.Label + " " + "AS_Breakthrough".Translate(), realmHediff.pawn.NameFullColored +" "+ realmHediff.CurStage.extraTooltip, AscensionDefOf.AS_CultivationBreakthroughMessage, realmHediff.pawn);
                        }
                    }else if (realmHediff.Severity >= maxSeverityCap && realmHediff.def == AscensionDefOf.BodyRealm)// if its maxcap and its body we move onto essence realms
                    {
                        Pawn pawn = realmHediff.pawn;
                        pawn.health.AddHediff(AscensionDefOf.EssenceRealm).Severity = 1;
                        pawn.health.RemoveHediff(realmHediff);
                        
                    }
                }
            }
        }

        public static void PreHeal(Pawn pawn)
        {
            AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {

                Hediff hediff_Injury = hediffs[i];
                //we dont want to remove a hediff that has already been removed, and we dont care about visibility as we know what we are removing.
                if (hediff_Injury != null)
                {
                    if (hediff_Injury.def.defName == "Malnutrition" || hediff_Injury.def.defName == "BloodLoss")
                    {
                        if (settings.logHealsBool)
                        {
                            Log.Message($"Prehealed: {hediff_Injury.def.defName} on {pawn.LabelShort}");
                        }
                        pawn.health.RemoveHediff(hediff_Injury);
                    }

                    if (hediff_Injury.def.defName == "RegenerationComa")
                    {
                        if (!pawn.health.ShouldBeDead())
                        {
                            if (settings.logHealsBool)
                            {
                                Log.Message($"Prehealed: {hediff_Injury.def.defName} on {pawn.LabelShort}");
                            }
                            pawn.health.RemoveHediff(hediff_Injury);
                        }
                    }
                }
            }
        }
    }
}
