using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class AS_CompProperties_AbilityLifespanCost : CompProperties_AbilityEffect
    {
        public AS_CompProperties_AbilityLifespanCost()
        {
            compClass = typeof(AS_CompAbilityEffect_AbilityLifespanCost);
        }

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return "AS_AbilityLifespanCost".Translate(cost.ToString("0.##").Named("COST"));
            yield break;
        }

        public float cost;
    }
}
