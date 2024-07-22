using RimWorld;

namespace Ascension
{
    public class AS_CompProperties_AddFuel : CompProperties_AbilityEffect
    {
        public float fuel = 100;
        public AS_CompProperties_AddFuel()
        {
            compClass = typeof(AS_CompAbilityEffect_AddFuel);
        }
    }
}
