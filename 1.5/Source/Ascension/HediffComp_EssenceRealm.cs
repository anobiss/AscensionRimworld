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
        private int ticksToQi;
        public int tickRate = 2500;
        public static readonly float[] maxQiRates = { 2f, 10f, 100f, 500f, 1000f, 10000f, 120000f };
        public static readonly float[] passiveQiBaseAmounts = { 10f, 100f, 1200f, 7000f, 12000f, 24000f, 77000f };
        public static readonly float[] passiveQiBaseSpeeds = { 1f, 1.5f, 2.7f, 3f, 4f, 5f, 7f };
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
                if (cultivatorHediff != null)
                {
                    UpdateRealmQiRecovery(true);
                }
            }
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            UpdateRealmQiRecovery(false);
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

        public void UpdateRealmQiRecovery(bool add)
        {
            if (cultivatorHediff != null)
            {
                if (qiPool == null)
                {
                    Pawn.health.AddHediff(AscensionDefOf.QiPool);
                    qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                }

                if (qiPool != null)  // Add null check for qiPool
                {
                    int tier = ((int)Math.Floor(parent.Severity));
                    float qiRecAmount = 0;
                    float qiRecSpeed = 0;
                    if (tier <= 7)
                    {
                        if (tier < 1)
                        {
                            qiRecAmount = passiveQiBaseAmounts[0];
                            qiRecSpeed = passiveQiBaseSpeeds[0];
                            AscensionUtilities.UpdateQiRecoveryAmount(cultivatorHediff);
                            AscensionUtilities.UpdateRealmMaxQi(0, qiPool);
                        }
                        else
                        {
                            qiRecAmount = passiveQiBaseAmounts[tier - 1];
                            qiRecSpeed = passiveQiBaseSpeeds[tier - 1];
                            AscensionUtilities.UpdateQiRecoveryAmount(cultivatorHediff);
                            AscensionUtilities.UpdateRealmMaxQi(tier - 1, qiPool);
                        }
                        AscensionUtilities.UpdateQiMax(qiPool);
                    }
                    else if (tier > 7)
                    {
                        qiRecAmount = passiveQiBaseAmounts[6];
                        qiRecSpeed = passiveQiBaseSpeeds[6];
                        AscensionUtilities.UpdateQiRecoveryAmount(cultivatorHediff);
                        AscensionUtilities.UpdateRealmMaxQi(6, qiPool);
                        AscensionUtilities.UpdateQiMax(qiPool);
                    }

                    if (add == true)
                    {
                        cultivatorHediff.qiRecoveryAmountBase += qiRecAmount;
                        cultivatorHediff.qiRecoverySpeedBase += qiRecSpeed;
                    }
                    else
                    {
                        cultivatorHediff.qiRecoveryAmountBase -= qiRecAmount;
                        cultivatorHediff.qiRecoverySpeedBase -= qiRecSpeed;
                    }
                }
            }
        }
    }
}
