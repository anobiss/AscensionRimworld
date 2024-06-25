using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace Ascension
{
    public class IngestionOutcomeDoer_RefineQi : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool, false) as QiPool_Hediff;
            Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
            if (qiPool != null && essenceHediff != null)
            {
                long qiCost = 2 + (qiPool.maxAmount / 10);// 10% + 2
                if (qiPool.amount >= qiCost)
                {
                    if (essenceHediff.progress < essenceHediff.maxProgress) // this is so we dont get the breakthroughpossible message over and over again
                    {
                        AscensionUtilities.TierProgress(pawn, AscensionDefOf.EssenceRealm, qiCost);
                        qiPool.amount -= qiCost;
                    }
                }
            }
        }
    }
}