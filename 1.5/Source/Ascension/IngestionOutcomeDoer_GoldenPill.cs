using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace Ascension
{
    public class IngestionOutcomeDoer_GoldenPill : IngestionOutcomeDoer
    {

        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            float num;
            if (amount > 0f)
            {
                if (noQuality == true)
                {
                    AscensionUtilities.IncreaseQi(pawn, amount);
                }
                else
                {
                    QualityCategory qc = new QualityCategory();
                    QualityUtility.TryGetQuality(ingested, out qc);
                    num = (amount * AscensionUtilities.GetQualityMultiplier((int)qc));
                    HediffDef QiPool = AscensionDefOf.QiPool;
                    AscensionUtilities.IncreaseQi(pawn, (int)Math.Floor(num));
                }
            }
        }
        public bool noQuality = false;
        public float amount;
    }
}