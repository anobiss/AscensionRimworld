using RimWorld;

namespace Ascension
{
    public class AS_CompProperties_DeathOnUse : CompProperties_AbilityEffect
    {
        public bool destroy = false;//for deleting the caster on use
        public AS_CompProperties_DeathOnUse()
        {
            compClass = typeof(AS_CompAbilityEffect_DeathOnUse);
        }
    }
}
