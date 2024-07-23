using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompProperties_ThingWall : CompProperties_AbilityEffect
    {
        public int wallHieght = 1;
        public int wallLWidth = 3;
        //public ThingDef wallThing = 
        public AS_CompProperties_ThingWall()
        {
            compClass = typeof(AS_CompAbilityEffect_ThingWall);
        }
    }
}
