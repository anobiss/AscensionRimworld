using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class HediffCompProperties_AddScrollAbility : HediffCompProperties
    {
        public List<AbilityDef> scrollCompAbilityDefList = new List<AbilityDef>(); // stores abilities given from scroll comps
        public int reqEssence = 0;
        public int reqBody = 0;
        public HediffCompProperties_AddScrollAbility()
        {
            compClass = typeof(HediffComp_AddScrollAbility);
        }
    }
}
