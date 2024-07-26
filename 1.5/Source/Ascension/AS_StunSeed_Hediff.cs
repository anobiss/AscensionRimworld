using RimWorld;
using System;
using Verse;

namespace Ascension
{
    public class AS_StunSeed_Hediff : Hediff
    {
        public int growTicks = 1250;//half hour to grow
        public override string SeverityLabel
        {
            get
            {
                return "AS_StunSeedSeverity".Translate(AscensionUtilities.TranslateSpeedHour((float)growTicks, true).Named("STUNTIME"));
            }
        }
        public override void PostTick()
        {
            base.PostTick();
            this.growTicks--;
            //Log.Message("grow ticks at"+growTicks);
            if (this.growTicks <= 0)
            {
                //Log.Message("stunning at" + growTicks);
                pawn.stances.stunner.StunFor(240,pawn);
                this.Severity = 0f;
            }
        }
        public override void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.growTicks, "growTicks", 1250, false);
            base.ExposeData();
        }
    }
}
