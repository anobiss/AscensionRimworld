
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
        //heals very common bad hediffs (malnutrition, bloodloss) first before the actual heal to prevent them from preventing the heal from fixing other hediffs.


        private int ticksToQi;

        public int tickRate = 2500;//every hour

        public static readonly float[] maxQiRates = { 2f, 10f, 100f, 500f, 1000f, 10000f, 120000f}; // max qi offset to set to when advancing first is tier 2
        public static readonly int[] passiveQiRates = { 1, 10, 100, 500, 1000, 3000, 12000}; //amount of qi generated per hour first is tier 2
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToQi--;
            if (this.ticksToQi <= 0)
            {
                Cultivator_Hediff cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                if (cultivatorHediff != null)
                {
                    cultivatorHediff.qiRecoverySpeed = (int)Math.Floor(cultivatorHediff.qiRecoverySpeedOffset + 1f);
                    tickRate = (int)Math.Floor(2500 / cultivatorHediff.qiRecoverySpeed);
                    int tier = ((int)Math.Floor(parent.Severity));
                    if (tier <= 7)
                    {
                        cultivatorHediff.qiRecoverySpeed = (int)Math.Floor(cultivatorHediff.qiRecoverySpeedOffset + 1f);
                        tickRate = (int)Math.Floor(2500 / cultivatorHediff.qiRecoverySpeed);
                        float qiMax;
                        int qiRate;
                        if (tier < 1)
                        {
                            cultivatorHediff.qiRecoveryAmount = passiveQiRates[1];
                            qiMax = maxQiRates[1];
                        }
                        else
                        {
                            cultivatorHediff.qiRecoveryAmount = passiveQiRates[tier - 1];
                            qiMax = maxQiRates[tier - 1];
                        }
                        //checks to make sure we dont explode the pawn passivley

                        QiPool_Hediff qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                        if (qiPool == null)
                        {

                            HediffSet hediffSet = Pawn.health.hediffSet;
                            //gives hediffs that r missing.
                            Pawn.health.AddHediff(AscensionDefOf.QiPool);
                            qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff; // set qiPool to new hediff to garentee qipool references
                        }
                        qiPool.maxAmountOffset = qiMax;
                        AscensionUtilities.UpdateQiMax(qiPool);
                        AscensionUtilities.IncreaseQi(Pawn, cultivatorHediff.qiRecoveryAmount, true);
                    }
                    else if (tier > 7)
                    {
                        QiPool_Hediff qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                        if (qiPool == null)
                        {
                            HediffSet hediffSet = Pawn.health.hediffSet;
                            //gives hediffs that r missing.
                            Pawn.health.AddHediff(AscensionDefOf.QiPool);
                            qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff; // set qiPool to new hediff to garentee qipool references
                        }
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