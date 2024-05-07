

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
    public class CompProperties_AbilityHediffToggle : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityHediffToggle()
        {
            compClass = typeof(CompAbilityEffect_HediffToggle);
        }
        public HediffDef hediffDef = null;
    }
}