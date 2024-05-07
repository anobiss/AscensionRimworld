using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class CompProperties_AbilityQiHeal : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityQiHeal()
        {
            this.compClass = typeof(CompAbilityEffect_QiHeal);
        }
    }
}
