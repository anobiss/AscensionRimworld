using Verse;

namespace Ascension
{
    public class CompConstantHeatPusher : ThingComp
    {
        public CompProperties_ConstantHeatPusher Props => (CompProperties_ConstantHeatPusher)props;

        public virtual bool ShouldPushHeatNow
        {
            get
            {
                if (parent.SpawnedOrAnyParentSpawned)
                {
                    return true;
                }
                return false;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(60) && ShouldPushHeatNow)
            {
                GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, Props.heat);
            }
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            if (ShouldPushHeatNow)
            {
                GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, Props.heat * 4.1666665f);
            }
        }
    }
}