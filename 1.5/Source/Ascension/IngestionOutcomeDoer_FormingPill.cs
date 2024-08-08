
using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using static HarmonyLib.Code;

namespace Ascension
{
    public class IngestionOutcomeDoer_FormingPill : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff == null && qiPool == null)
            {
                pawn.health.AddHediff(AscensionDefOf.Cultivator);
                qiPool = HediffMaker.MakeHediff(AscensionDefOf.QiPool, pawn, null) as QiPool_Hediff;
                qiPool.amount = 1;
                pawn.health.AddHediff(qiPool);
                AscensionUtilities.UpdateQiMax(qiPool);
                cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            }
            if (cultivatorHediff != null)
            {
                if (cultivatorHediff.Foundation < cultivatorHediff.fMax)
                {
                    cultivatorHediff.Foundation = cultivatorHediff.fMax;
                }
            }
        }
    }
}