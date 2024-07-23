using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompProperties_ThingCircle : CompProperties_AbilityEffect
    {
        public float innerCircleRadius = 2;//circle within the thingcircle where the circlething does not spawn so that the circle is hallow
        public float outerCircleRadius = 3;//radius in which things are place in a circle around the targer
        public ThingDef circleThing = AscensionDefOf.Slate;
        public AS_CompProperties_ThingCircle()
        {
            compClass = typeof(AS_CompAbilityEffect_ThingCircle);
        }
    }
}
