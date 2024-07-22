using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompAbilityEffect_AddFuel : CompAbilityEffect
    {
        public new AS_CompProperties_AddFuel Props => (AS_CompProperties_AddFuel)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            ChargeBattery(target);
        }

        private void ChargeBattery(LocalTargetInfo target)
        {
            Pawn pawn = parent.pawn;
            Thing battery = target.Thing;
            CompRefuelable fuelComp = battery.TryGetComp<CompRefuelable>();
            if (fuelComp != null)
            {
                fuelComp.Refuel(Props.fuel);

                //fake explosion
                //GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 4.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0, 0, null, false);
            }
        }
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
