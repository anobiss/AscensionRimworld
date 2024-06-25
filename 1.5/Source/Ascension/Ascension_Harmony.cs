

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using static HarmonyLib.Code;
using OpCodes = System.Reflection.Emit.OpCodes;



namespace Ascension
{
    [StaticConstructorOnStartup]
    public static class AscensionHarmony
    {
        static AscensionHarmony()
        {
            Harmony harmony = new Harmony ("anobis.epicmods.ascension");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Harmony.DEBUG = false;

            harmony.Patch(original: AccessTools.Method(type: typeof(PawnGenerator), name: "GenerateInitialHediffs"), prefix: null, postfix: null,
                transpiler: new HarmonyMethod(typeof(AscensionHarmony), nameof(BookIconTranspiler)));

        }
        public static IEnumerable<CodeInstruction> BookIconTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];


                if (instruction.opcode == OpCodes.Call && instruction.operand == AccessTools.Method(type: typeof(PawnAddictionHediffsGenerator), name: "GenerateAddictionsAndTolerancesFor"))
                {
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_0);
                    yield return new CodeInstruction(opcode: OpCodes.Call, operand: AccessTools.Method(type: typeof(AscensionHarmony), name: nameof(AscensionHarmony.ChanceAddAscension)));
                }
                yield return instruction;
            }
        }
        public static void ChanceAddAscension(Pawn pawn)
        {
            //some settings for psuedo immortality and ascendancy chance should be done as a game condition later instead of an actual setting to discourage unbalanced cheese and allow specific items to change them instead
            AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
            if (pawn != null)
            {
                if (pawn.health != null)
                {
                    bool condition = pawn.health.hediffSet != null;
                    if (settings.humanoidOnlyBool)
                    {
                        condition = (pawn.health.hediffSet != null) && (pawn.def.race.Humanlike);
                    }
                    if (!settings.machineCultivatorBool)
                    {
                        if (pawn.def.race.IsMechanoid || !pawn.def.race.IsFlesh)
                        {
                            condition = false;
                        }
                    }
                    if (condition)
                    {
                        Random rnd = new Random();
                        //has to be float for fine conrols over chance in settings
                        float randpi = Rand.Range(0, 1f);
                        float randC = Rand.Range(0, 1f);
                        float randER = Rand.Range(0, 1f);

                        float chanceC = settings.CultivatorChance;
                        float chanceER = settings.EssenceChance;
                        float chancePC = settings.PCChance;
                        float chancePI = settings.PIChance;

                        int maxGoldenCore = (int)settings.GoldenCoreMax;

                        float chanceA = settings.AbilityChance;
                        Cultivator_Hediff cultivatorHediff;
                        //if the chance number is greater or equal to the rng number it gives the hediff, this makes it so a 0.07 chance would be a 7% chance.

                        if (randC <= chanceC)
                        {
                            cultivatorHediff = HediffMaker.MakeHediff(AscensionDefOf.Cultivator, pawn) as Cultivator_Hediff;
                            int randRealmStage = rnd.Next(1, 4);
                            //if they failed to get psuedo immortality roll for random cultivation realms.
                            randC = Rand.Range(0, 1f);
                            HediffDef RealmDef = AscensionDefOf.BodyRealm;
                            if (randER <= chanceER)
                            {
                                RealmDef = AscensionDefOf.EssenceRealm;
                                if (randRealmStage >= 3)
                                {
                                    cultivatorHediff.goldenCoreScore = rnd.Next(1, maxGoldenCore/10);
                                }
                            }
                            if (randC <= chancePC)
                            {
                                //upgrade to strong rand realms and to essence realm if powerful
                                randRealmStage = rnd.Next(3, 6);
                                RealmDef = AscensionDefOf.EssenceRealm;

                                cultivatorHediff.goldenCoreScore = rnd.Next(20000, maxGoldenCore);
                            }
                            else
                            {
                                randRealmStage = rnd.Next(1, 3);
                            }


                            //if they are essence realm and the severity is three or above we should generate a random golden core.
                            //higher random golden core score if they are powerful cultivatior
                            if (RealmDef == AscensionDefOf.EssenceRealm && randRealmStage >= 3)
                            {
                                cultivatorHediff.goldenCoreScore = rnd.Next(1, maxGoldenCore/10);
                            }


                            Realm_Hediff realmHediff = HediffMaker.MakeHediff(RealmDef, pawn) as Realm_Hediff;

                            if (randpi <= chancePI)
                            {
                                realmHediff = HediffMaker.MakeHediff(AscensionDefOf.EssenceRealm, pawn) as Realm_Hediff;
                                cultivatorHediff.goldenCoreScore = rnd.Next(maxGoldenCore, maxGoldenCore*4);
                                randpi = Rand.Range(0, 1f);
                                //0.1f makes it a 10% chance for a high level
                                if (randpi <= 0.1f)
                                {
                                    //profound immortal.
                                    realmHediff.Severity = 7;
                                }
                                else
                                {
                                    //psuedo immortal
                                    realmHediff.Severity = 6;
                                }
                            }
                            else
                            {
                                realmHediff.Severity = randRealmStage;
                            }


                            pawn.health.AddHediff(cultivatorHediff);
                            pawn.health.AddHediff(AscensionDefOf.QiPool);
                            pawn.health.AddHediff(realmHediff);
                            AscensionUtilities.UpdateMaxProg(realmHediff);
                            realmHediff.progress = AscensionUtilities.NextLong(rnd,0, realmHediff.maxProgress + 1);


                            float randA = Rand.Range(0, 1f);//only need to roll if its a cultivator
                            //now do random abilities here
                            if (randA <= chanceA)
                            {
                                if (AscensionStaticStartUtils.ScrollHediffList == null)
                                {
                                    //Log.Message("null scrollHediffList");
                                }
                                if (AscensionStaticStartUtils.ScrollHediffList.EnumerableNullOrEmpty())
                                {
                                    //Log.Message("empty or null scrollHediffList");
                                }

                                HashSet<HediffDef> alreadyAdded = new HashSet<HediffDef>();//this keeps stack of already added stuff.
                                //use randA again to scale the chance for more with the setting
                                int scrollAmount = rnd.Next(1, 3);//just do 1-2 for now
                                //Log.Message("adding scrolls amount "+scrollAmount);
                                for (int i = 0; i < scrollAmount; i++)
                                {
                                    //Log.Message("adding scroll");
                                    //give random ability scroll and its abilities here
                                    HashSet<HediffDef> randomScrollSet = new HashSet<HediffDef>(AscensionStaticStartUtils.ScrollHediffList); // since readonly now 
                                    List<HediffDef> scrollList = new(AscensionStaticStartUtils.ScrollHediffList);

                                    if (!randomScrollSet.NullOrEmpty())
                                    {
                                        //remove already learned
                                        if (!alreadyAdded.NullOrEmpty()) // we only need to do this if alreadyadded isnt empty/null and theres scrollhediffs to use.
                                        {
                                            randomScrollSet.ExceptWith(alreadyAdded);
                                        }else
                                        {
                                            //Log.Message("nothing learned yet");
                                        }



                                        //remove scrolls that require higher realms
                                        //null or empty check incase already learned removed everything
                                        if (!randomScrollSet.NullOrEmpty())
                                        {
                                            List<HediffDef> randomScrollSetReqList = new List<HediffDef>(randomScrollSet);//creates copy to clean up during and set after for loop
                                            for (int reqi = randomScrollSet.Count - 1; reqi >= 0; reqi--)
                                            {
                                                Scroll_Hediff randomScrollHediff = HediffMaker.MakeHediff(randomScrollSetReqList[reqi], pawn) as Scroll_Hediff;
                                                HediffComp_AddScrollAbility scrollAbilityComp = randomScrollHediff.TryGetComp<HediffComp_AddScrollAbility>();
                                                if (scrollAbilityComp != null)
                                                {
                                                    //checks if pawn has required realms
                                                    if (scrollAbilityComp.Props.reqEssence > 0)
                                                    {
                                                        if (RealmDef == AscensionDefOf.BodyRealm)
                                                        {
                                                            randomScrollSetReqList.RemoveAt(reqi);
                                                        }else if ((int)Math.Floor(realmHediff.Severity) < scrollAbilityComp.Props.reqEssence)
                                                        {
                                                            randomScrollSetReqList.RemoveAt(reqi);
                                                        }
                                                    }else if (scrollAbilityComp.Props.reqBody > 0)
                                                    {
                                                        if (RealmDef != AscensionDefOf.EssenceRealm)
                                                        {
                                                            if ((int)Math.Floor(realmHediff.Severity) < scrollAbilityComp.Props.reqBody)
                                                            {
                                                                randomScrollSetReqList.RemoveAt(reqi);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            //sets scrollset to new one after removing all ones with too high realm reqs
                                            randomScrollSet = new HashSet<HediffDef>(randomScrollSetReqList);//maybe this is returning null?
                                        }


                                        //another null check after removing everything we cant give them incase its litteraly everything
                                        if (!randomScrollSet.NullOrEmpty())
                                        {
                                            //now here we can give scrolls after removing bad ones

                                            //we minus scrollhedifflist by the already learned to make a hashset to send to random ability scroll
                                            //random ability scroll gets a random ability from it
                                            HediffDef ScrollDef = randomScrollSet.RandomElement();//gets random loaded scrollhediff def that isnt already learned
                                            //Log.Message("chose " + ScrollDef.label);
                                            alreadyAdded.Add(ScrollDef);//adds it to already added list to not roll it again.
                                            Scroll_Hediff scrollHediff = HediffMaker.MakeHediff(ScrollDef, pawn) as Scroll_Hediff;//makes it into hediff for further refrencing for abilities
                                            HediffComp_AddScrollReq scrollReqComp = scrollHediff.TryGetComp<HediffComp_AddScrollReq>();
                                            //Log.Message("scroll hediff is" + scrollHediff.def.label);
                                            if (scrollHediff == null)
                                            {
                                                //Log.Message("no scroll hediff");
                                            }
                                            if (scrollReqComp != null)
                                            {
                                                //this part gets required scroll arts and hediffs
                                                if (scrollReqComp.Props.reqHediffsList != null && scrollReqComp.Props.reqHediffsList.Count > 0)
                                                {

                                                    for (int ri = 0; ri < scrollReqComp.Props.reqHediffsList.Count; ri++)
                                                    {

                                                        if (!pawn.health.hediffSet.HasHediff(scrollReqComp.Props.reqHediffsList[ri]))// makes it not give req hediff to pawn that already has it
                                                        {
                                                            HediffDef reqHediffDefTemp = scrollReqComp.Props.reqHediffsList[ri];
                                                            pawn.health.AddHediff(reqHediffDefTemp); //might not get first req hediff?
                                                                                                     //Log.Message("added scroll req hediff"+ reqHediffDefTemp.label);
                                                            alreadyAdded.Add(reqHediffDefTemp);
                                                        }
                                                    }
                                                }

                                                if (scrollReqComp.Props.reqHediffsList == null || scrollReqComp.Props.reqHediffsList.Count <= 0)
                                                {
                                                    //Log.Message("no ability list");
                                                }
                                            }

                                            pawn.health.AddHediff(scrollHediff);//add hediff first to check if we already have it when applying req hediffs.
                                        }
                                    }
                                    else
                                    {
                                        //Log.Message("Ascension error: couldnt get ability not already learned or too high realm req for pawn "+body.Label+essence.Label);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class HotSwappableAttribute : Attribute
    {
    }
}

