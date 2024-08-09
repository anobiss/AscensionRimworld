
using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace Ascension
{
    public class IngestionOutcomeDoer_LifePill : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            QualityUtility.TryGetQuality(ingested, out QualityCategory qc);
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff == null)
            {
                pawn.health.AddHediff(AscensionDefOf.Cultivator);
                cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            }
            cultivatorHediff.lifespan += amount * AscensionUtilities.GetQualityMultiplier((int)qc);
        }
        public float amount;
    }
}