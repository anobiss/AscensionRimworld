using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompProperties_ThingWall : CompProperties_AbilityEffect
    {
        public int wallHieght = 3;
        public int wallLWidth = 1;
        public ThingDef wallThing = AscensionDefOf.Slate;
        public AS_CompProperties_ThingWall()
        {
            compClass = typeof(AS_CompAbilityEffect_ThingWall);
        }
    }
}
