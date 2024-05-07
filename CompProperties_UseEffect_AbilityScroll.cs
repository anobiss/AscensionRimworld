using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class CompProperties_UseEffect_AbilityScroll : CompProperties_UseEffect
    {
        public CompProperties_UseEffect_AbilityScroll()
        {
            this.compClass = typeof(CompUseEffect_AbilityScroll);
        }
        public HediffDef scrollHediffDef;
    }

}
