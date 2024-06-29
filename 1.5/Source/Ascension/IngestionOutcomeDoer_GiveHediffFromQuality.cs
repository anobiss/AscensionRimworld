using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Ascension
{
    public class IngestionOutcomeDoer_GiveHediffFromQuality : IngestionOutcomeDoer
    {
        private float GetQualityMultiplier(Thing thing)
        {
            QualityCategory qc = new QualityCategory();
            QualityUtility.TryGetQuality(thing, out qc);

            if (((int)qc) == 0)
            {
                return 0.5f;
            }
            else if (((int)qc) == 1)
            {
                return 1f;
            }
            else if ((((int)qc) > 1) && (((int)qc) < 6))
            {
                return (int)qc;
            }
            else if (((int)qc) == 6)
            {
                return 10f;
            }
            return 1f;
        }
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            float num;
            if (amount > 0f)
            {
                num = (amount * GetQualityMultiplier(ingested));
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