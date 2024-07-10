using System;
using Verse;

namespace Ascension
{
    public class HediffComp_QiRecoveryBase : HediffComp
    {
        //this comp does qi recovery, it updates the amount and speed using the utilities class then adds qi at according rates
        //this comp should only be attatched to qi pool

        private int ticksToQi;
        public int tickRate = 2500;//in game hour
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        Cultivator_Hediff cultivatorHediff;
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            if (Pawn.health.hediffSet != null)
            {
                cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            }
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
        }
        public override void CompPostMake()
        {
            base.CompPostMake();
            if (Pawn.health.hediffSet != null)
            {
                cultivatorHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToQi--;
            if (this.ticksToQi <= 0)
            {
                if (cultivatorHediff != null)
                {
                    AscensionUtilities.UpdateQiRecoveryAmount(cultivatorHediff);
                    tickRate = (int)Math.Floor(2500 / AscensionUtilities.UpdateQiRecoverySpeed(cultivatorHediff));
                    AscensionUtilities.IncreaseQi(Pawn, AscensionUtilities.UpdateQiRecoveryAmount(cultivatorHediff), true);
                }
                ticksToQi = tickRate;
            }
        }
        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToQi, "ticksToQi", 0, false);
        }
        public override string CompDebugString()
        {
            return "ticksToQi: " + this.ticksToQi;
        }
    }
}
