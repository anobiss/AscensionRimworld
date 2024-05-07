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
    public class CompProperties_AbilityHediffGiver : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityHediffGiver()
        {
            compClass = typeof(CompAbilityEffect_HediffGiver);
        }

        public float minSeverity = 0f;
        public float maxSeverity = 0f;
        public HediffDef hediffDef = null;
    }
}
