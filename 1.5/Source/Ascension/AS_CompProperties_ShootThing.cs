
using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompProperties_ShootThing : CompProperties_AbilityEffect
    {
        public ThingDef shotThing = AscensionDefOf.Slate;
        public AS_CompProperties_ShootThing()
        {
            compClass = typeof(AS_CompAbilityEffect_ShootThing);
        }
    }
}
