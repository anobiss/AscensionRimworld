using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class CompProperties_AbilityQiCost : CompProperties_AbilityEffect
    {

        public CompProperties_AbilityQiCost()
        {
            compClass = typeof(CompAbilityEffect_AbilityQiCost);
        }

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return "AS_AbilityQiCost".Translate(cost.Named("COST"));
            yield break;
        }
        public float minSeverity = 0f;
        public HediffDef reqHediffDef = null;

        public bool cannotCastIfMissing = true;

        public int cost;
    }
}
