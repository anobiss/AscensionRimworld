
using RimWorld;
using System;
using Verse;


namespace Ascension
{
    public class HediffComp_QiRecovery : HediffComp
    {
        protected HediffCompProperties_QiRecovery Props => (HediffCompProperties_QiRecovery)props;

        public override void CompPostMake()
        {
            base.CompPostMake();
            //parent.LabelColor = Props.labelColor;
            //set label color
            Cultivator_Hediff cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                cultivatorHediff.cultivationSpeedOffset = Props.cultivationSpeedOffset + 1;
                cultivatorHediff.qiRecoverySpeedOffset = Props.offset;
            }
        }
        private int ticksToRemove = 180000;//3 days
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToRemove--;
            if (this.ticksToRemove <= 0)
            {
                parent.Severity = 0;
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