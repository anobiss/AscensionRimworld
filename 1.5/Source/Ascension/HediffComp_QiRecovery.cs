using RimWorld;
using Verse;
namespace Ascension
{
    public class HediffComp_QiRecovery : HediffComp
    {
        public HediffCompProperties_QiRecovery Props => (HediffCompProperties_QiRecovery)props;
        private int ticksToRemove = 180000;//3 days
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (Props.temp == true)
            {
                this.ticksToRemove--;
                if (this.ticksToRemove <= 0)
                {
                    parent.Severity = 0;
                }
            }

        }
        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToRemove, "ticksToRemove", 180000, false);
        }
        public override string CompDebugString()
        {
            return "ticksToRemove: " + this.ticksToRemove;
        }
    }
}