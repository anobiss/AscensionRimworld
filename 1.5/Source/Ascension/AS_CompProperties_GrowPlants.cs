using RimWorld;

namespace Ascension
{
    public class AS_CompProperties_GrowPlants : CompProperties_AbilityEffect
    {
        public float growRadius = 1f;
        public float growDays = 7f;
        public AS_CompProperties_GrowPlants()
        {
            compClass = typeof(AS_CompAbilityEffect_GrowPlants);
        }
    }
}