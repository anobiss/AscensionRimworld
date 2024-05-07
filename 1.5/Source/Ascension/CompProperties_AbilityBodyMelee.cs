using RimWorld;
using Verse;

namespace Ascension
{
    public class CompProperties_AbilityBodyMelee : CompProperties_AbilityEffect
    {
        public ThingDef projectileDef;
        public int damage;

        public CompProperties_AbilityBodyMelee()
        {
            compClass = typeof(CompAbilityEffect_BodyMelee);
        }
    }
}
