
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class CompProperties_AbilityManifestSpiritSword : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityManifestSpiritSword()
        {
            this.compClass = typeof(CompAbilityEffect_ManifestSpiritSword);
        }
    }
}
