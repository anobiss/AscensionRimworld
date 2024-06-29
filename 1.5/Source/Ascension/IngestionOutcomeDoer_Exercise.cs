
using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Ascension
{
    public class IngestionOutcomeDoer_Exercise : IngestionOutcomeDoer
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
            float num = ((int)Math.Floor((float)amount * GetQualityMultiplier(ingested)));
            Realm_Hediff bodyHediff = (Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm);
            if (bodyHediff != null)
            {
                num += bodyHediff.maxProgress / 100;//for adding 1 percent body to amount
            }
            AscensionUtilities.TierProgress(pawn, AscensionDefOf.BodyRealm, num);
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

        public float amount = 10;
    }
}