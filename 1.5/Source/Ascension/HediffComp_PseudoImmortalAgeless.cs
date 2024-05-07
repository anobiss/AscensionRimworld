using RimWorld;
using System;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class HediffComp_PseudoImmortalAgeless : HediffComp
    {
        private int ticksToAgeCheck = 10000;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (parent.Severity >= 6) //make sure they immortal
            {
                this.ticksToAgeCheck--;
                if (this.ticksToAgeCheck <= 0)
                {
                    this.parent.pawn.ageTracker.AgeBiologicalTicks = 75600000;
                    this.parent.pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
                }
            }
        }

        public override void CompPostMake()
        {
            if (parent.Severity >= 6)//only do if psuedo immortal or above
            {
                if (parent.pawn.ageTracker.AgeBiologicalYears > 21)
                {
                    this.parent.pawn.ageTracker.AgeBiologicalTicks = 75600000;
                    this.parent.pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
                }
            }
        }
    }
}