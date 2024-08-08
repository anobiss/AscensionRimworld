using RimWorld;
using System;
using Verse;
using Verse.Noise;
using static HarmonyLib.Code;

namespace Ascension
{
    public class HeavenlyTribulation_Hediff : HediffWithComps
    {
        private QiPool_Hediff qiPool;
        private Cultivator_Hediff cultivatorHediff;
        private float qiOffset;
        private float strengthOffset;
        private float speedOffset;
        private float convertedQi = 0f;//used only for the cultivation reward.
        private float Qi;
        private float Strength;
        private float Speed;
        private int ticksTilStrike;
        private int ticksTilStart = 180000; // 3 days
        private static readonly int baseTicks = 2500;//every hour
        public override string SeverityLabel
        {
            get
            {
                return "AS_HeavenlyTribulationStats".Translate(qiOffset.ToString("0.#").Named("QIOFFSET"), strengthOffset.ToString("0.#").Named("STRENGTHOFFSET"), 
                    speedOffset.ToString("0.#").Named("SPEEDOFFSET"), Qi.ToString("0.#").Named("QI"), Strength.ToString("0.#").Named("STRENGTH"), Speed.ToString("0.#").Named("SPEED"), AscensionUtilities.TranslateSpeedHour(Speed).Named("TRANSLATEDSPEED"), AscensionUtilities.TranslateSpeedHour(ticksTilStart, true).Named("TRANSLATEDTIMELEFT"));
            }
        }
        public override void PostTick()
        {
            base.PostTick();
            if (ticksTilStart <= 0)
            {
                this.ticksTilStrike--;
                if (this.ticksTilStrike <= 0)
                {
                    Map map = pawn.Map;
                    if (map != null)
                    {
                        ticksTilStrike = ResetStrikeTicks();
                        qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                        cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                        if (qiPool != null && cultivatorHediff != null)
                        {
                            if (qiPool.amount >= Strength)
                            {
                                pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_FakeLightningStrike(map, pawn.Position));
                                if (Strength > Qi)
                                {
                                    RollDivineBreath();
                                    
                                    qiPool.amount -= Strength;
                                    convertedQi += Strength;
                                    CultivationReward(convertedQi);
                                    Severity = 0;
                                }else
                                {
                                    qiPool.amount -= Strength;
                                    convertedQi += Strength;
                                    Qi -= Strength;
                                }
                            }
                            else
                            {
                                pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(map, pawn.Position));
                                if (Strength/4 > Qi)
                                {
                                    Severity = 0;
                                }else
                                {
                                    Qi -= Strength / 4;
                                }
                            }
                        }
                    }
                }
            }else
            {
                this.ticksTilStart--;
            }
        }

        private void CultivationReward(float amount)
        {
            RollDivineBreath();
            Realm_Hediff essenceRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
            Realm_Hediff bodyRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;

            HediffDef realmDef = essenceRealm.def;
            if (essenceRealm != null)
            {
                realmDef = essenceRealm.def;
                if (amount > essenceRealm.maxProgress)
                {
                    essenceRealm.progress = essenceRealm.maxProgress;
                }else
                {
                    essenceRealm.progress += amount;
                }

            }else if (bodyRealm != null)
            {
                realmDef = bodyRealm.def;
                if (amount > bodyRealm.maxProgress)
                {
                    bodyRealm.progress = bodyRealm.maxProgress;
                }
                else
                {
                    bodyRealm.progress += amount;
                }
            }
            AscensionUtilities.TierProgress(pawn, realmDef, amount);
            convertedQi = 0f;
        }
        private void RollDivineBreath()
        {
            Random rnd = new Random();
            //10% chance to drop divine breath when defeated.
            if ((float)rnd.NextDouble() > 0.9f)
            {
                GenPlace.TryPlaceThing(ThingMaker.MakeThing(AscensionDefOf.AS_HeavensBreath), pawn.Position, pawn.Map, ThingPlaceMode.Near);
            }

        }
        private static AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            qiOffset = AscensionUtilities.UpdateTribulationQiOffset(this);
            strengthOffset = AscensionUtilities.UpdateTribulationStrengthOffset(this);
            speedOffset = AscensionUtilities.UpdateTribulationSpeedOffset(this);
            Qi = qiOffset * AscensionUtilities.UpdateQiMax(qiPool);
            Strength = strengthOffset * (Qi * 0.05f) * (AscensionUtilities.UpdateQiRecoverySpeed(qiPool)+1);
            Speed = speedOffset * AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
            if (PawnUtility.ShouldSendNotificationAbout(pawn))
            {
                if (!settings.disableHTLetters)
                {
                    Find.LetterStack.ReceiveLetter("AS_HTLetter".Translate(), "AS_HTLetterDesc".Translate(pawn.NameFullColored.Named("PAWN")), AscensionDefOf.AS_HeavenlyTribulationMessage, pawn);
                }
            }
        }
        private int ResetStrikeTicks()
        {
            int strikeTicks = 2500; // hour by default
            if (Speed > 0)
            {
                strikeTicks = (int)(baseTicks / Speed);
            }
            return strikeTicks;
        }
        public override void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksTilStart, "ticksTilStart", 180000, false);//3 days
            Scribe_Values.Look<int>(ref this.ticksTilStrike, "ticksTilStrike", ResetStrikeTicks(), false);
            Scribe_Values.Look<float>(ref Qi, "Qi", 100f, false);
            Scribe_Values.Look<float>(ref Strength, "Strength", 1f, false);
            Scribe_Values.Look<float>(ref Speed, "Speed", 1f, false);
            base.ExposeData();
        }
    }
}
