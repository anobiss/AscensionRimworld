using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Verse;

namespace Ascension
{
    public class HediffComp_EssenceRealm : HediffComp
    {
        //migrate max and passive logic to utilities. 
        private int ticksToQi;
        public int tickRate = 2500;
        public static readonly float[] maxQiRates = { 2f, 10f, 100f, 500f, 1000f, 10000f, 120000f };
        public static readonly int[] passiveQiRates = { 1, 10, 100, 500, 1000, 3000, 12000 };
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        QiPool_Hediff qiPool;
        Cultivator_Hediff cultivatorHediff;
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            if (Pawn.health.hediffSet != null)
            {
                cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            }
        }
        public override void CompPostMake()
        {
            base.CompPostMake();
            if (Pawn.health.hediffSet != null)
            {
                cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToQi--;
            if (this.ticksToQi <= 0)
            {
                if (cultivatorHediff != null)
                {
                    if (qiPool == null)
                    {
                        Pawn.health.AddHediff(AscensionDefOf.QiPool);
                        qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                    }
                    cultivatorHediff.qiRecoverySpeed = (int)Math.Floor(cultivatorHediff.qiRecoverySpeedOffset + 1f);
                    tickRate = (int)Math.Floor(2500 / cultivatorHediff.qiRecoverySpeed);
                    int tier = ((int)Math.Floor(parent.Severity));
                    if (tier <= 7)
                    {
                        cultivatorHediff.qiRecoverySpeed = (int)Math.Floor(cultivatorHediff.qiRecoverySpeedOffset + 1f);
                        tickRate = (int)Math.Floor(2500 / cultivatorHediff.qiRecoverySpeed);
                        if (tier < 1)
                        {
                            cultivatorHediff.qiRecoveryAmount = passiveQiRates[1];
                            AscensionUtilities.UpdateRealmMaxQi(1, qiPool);
                        }
                        else
                        {
                            cultivatorHediff.qiRecoveryAmount = passiveQiRates[tier - 1];
                            AscensionUtilities.UpdateRealmMaxQi(tier - 1, qiPool);
                        }
                        AscensionUtilities.UpdateQiMax(qiPool);
                        AscensionUtilities.IncreaseQi(Pawn, cultivatorHediff.qiRecoveryAmount, true);
                    }
                    else if (tier > 7)
                    {
                        if (qiPool == null)
                        {
                            Pawn.health.AddHediff(AscensionDefOf.QiPool);
                            qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                        }
                        AscensionUtilities.UpdateRealmMaxQi(6, qiPool);
                        AscensionUtilities.UpdateQiMax(qiPool);
                        AscensionUtilities.IncreaseQi(Pawn, passiveQiRates[6], true);
                    }
                }
                ticksToQi = tickRate;
            }
        }
        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToQi, "ticksToQi", 0, false);
        }
        public override string CompDebugString()
        {
            return "ticksToQi: " + this.ticksToQi;
        }
    }
}
