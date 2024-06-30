using RimWorld;
using Verse;
namespace Ascension
{
    public class HediffComp_QiRecovery : HediffComp
    {
        public HediffCompProperties_QiRecovery Props => (HediffCompProperties_QiRecovery)props;
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            AddRecovery();
        }
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            RemoveRecovery();
        }
        private int ticksToRemove = 180000;//3 days
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToRemove--;
            if (this.ticksToRemove <= 0)
            {
                RemoveRecovery();
            }
        }
        public void AddRecovery() 
        {
            Cultivator_Hediff cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                if (Props.spirit == true)
                {
                    float severity = parent.Severity;
                    cultivatorHediff.qiRecoverySpeedOffset += AscensionUtilities.spiritPillOffsetRates[(int)severity - 1];
                }
                else
                {
                    cultivatorHediff.cultivationSpeedOffset += Props.cultivationSpeedOffset;
                    cultivatorHediff.qiRecoverySpeedOffset += Props.offset;
                }
            }
        }

        public void RemoveRecovery()//public so we can remove it when we are changing severity
        {
            if (parent != null)
            {
                parent.Severity = 0;
            }
            Cultivator_Hediff cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                if (Props.spirit == true)
                {
                    float severity = parent.Severity;
                    cultivatorHediff.qiRecoverySpeedOffset -= AscensionUtilities.spiritPillOffsetRates[(int)severity - 1];
                }
                else
                {
                    cultivatorHediff.cultivationSpeedOffset -= Props.cultivationSpeedOffset;
                    cultivatorHediff.qiRecoverySpeedOffset -= Props.offset;
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