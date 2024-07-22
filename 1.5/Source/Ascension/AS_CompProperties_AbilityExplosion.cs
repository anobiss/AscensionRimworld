using RimWorld;

namespace Ascension
{
    public class AS_CompProperties_AbilityExplosion : CompProperties_AbilityEffect
    {
        public float radius = 5f;
        public int damage = 10;
        public AS_CompProperties_AbilityExplosion()
        {
            compClass = typeof(AS_CompAbilityEffect_Explosion);
        }
    }
}
