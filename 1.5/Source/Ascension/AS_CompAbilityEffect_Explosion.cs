using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompAbilityEffect_Explosion : CompAbilityEffect
    {
        public new AS_CompProperties_AbilityExplosion Props => (AS_CompProperties_AbilityExplosion)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            StrikeLightning(target);
        }

        private void StrikeLightning(LocalTargetInfo target)
        {
            Pawn pawn = parent.pawn;
            GenExplosion.DoExplosion(target.Cell, Find.CurrentMap, Props.radius, DamageDefOf.Bomb, null, Props.damage, -1f, null, null, null, null, null, 0, 0, null, false);
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
