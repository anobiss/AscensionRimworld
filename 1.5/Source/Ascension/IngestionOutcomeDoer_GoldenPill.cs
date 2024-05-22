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
            int num;
            if (hediffDef == AscensionDefOf.QiPool)
            {
                AscensionUtilities.IncreaseQi(pawn, amount);
            }
            else
            {
                num = amount;
                Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
                hediff.Severity = (float)num/100;
                pawn.health.AddHediff(hediff, null, null, null);
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
                IEnumerator<StatDrawEntry> enumerator = null;
            }
            yield break;
        }

        public HediffDef hediffDef;

        public int amount;

        public int tier = -1;

        public ChemicalDef toleranceChemical;
    }
}