using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompAbilityEffect_ChargeBattery : CompAbilityEffect
    {
        public new AS_CompProperties_ChargeBattery Props => (AS_CompProperties_ChargeBattery)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            ChargeBattery(target);
        }

        private void ChargeBattery(LocalTargetInfo target)
        {
            Pawn pawn = parent.pawn;
            Thing battery = target.Thing;
            CompPowerBattery batteryComp = battery.TryGetComp<CompPowerBattery>();
            if (batteryComp != null)
            {
                batteryComp.AddEnergy(Props.charge);
                pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_FakeLightningStrike(pawn.Map, target.Cell));
            }
        }
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
