using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Ascension
{
    public class IngestionOutcomeDoer_GiveHediffFromQuality : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            float num;
            if (amount > 0f)
            {
                QualityCategory qc = new QualityCategory();
                QualityUtility.TryGetQuality(ingested, out qc);
                num = (amount * AscensionUtilities.GetQualityMultiplier((int)qc));
                HediffDef QiPool = AscensionDefOf.QiPool;

                if (this.hediffDef == QiPool)// do qi adding logic here
                {
                    AscensionUtilities.IncreaseQi(pawn, (int)Math.Floor(num));
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
                    hediff.Severity = num;
                    pawn.health.AddHediff(hediff, null, null, null);
                }
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (parentDef.IsDrug && this.chance >= 1f)
            {
                foreach (StatDrawEntry statDrawEntry in this.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return statDrawEntry;
                }
            }
            yield break;
        }

        public HediffDef hediffDef;

        public float amount;

        public ChemicalDef toleranceChemical;
    }
}