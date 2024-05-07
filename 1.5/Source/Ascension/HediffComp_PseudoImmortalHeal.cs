using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Verse;

namespace Ascension
{

    public class HediffComp_PseudoImmortalHeal : HediffComp
    {
        //heals very common bad hediffs (malnutrition, bloodloss) first before the actual heal to prevent them from preventing the heal from fixing other hediffs.


        private int ticksToHeal;  

        public static readonly int[] tickRates = { 2500, 42};
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (parent.Severity >=6) //make sure they immortal
            {
                this.ticksToHeal--;
                if (this.ticksToHeal <= 0)
                {
                    AscensionUtilities.PreHeal(this.Pawn);
                    HealthUtility.FixWorstHealthCondition(Pawn);
                    if (settings.logHealsBool)
                    {
                        Log.Message($"Healed pawn {Pawn.LabelShort}");
                    }
                    this.Reset();
                }
            }
        }

        private void Reset()
        {
            if (parent.Severity >= 7)
            {
                ticksToHeal = tickRates[1];
            } else if (parent.Severity >= 6 && parent.Severity <7)
            {
                ticksToHeal = tickRates[0];
            }
            if (settings.logHealsBool)
            {
                Log.Message($"Next heal will take {ticksToHeal / 2500} hours.");
            }
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
        }

        public override string CompDebugString()
        {
            return "ticksToHeal: " + this.ticksToHeal;
        }
    }
}