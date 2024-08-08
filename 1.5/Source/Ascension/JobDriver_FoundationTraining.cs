

using System;
using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace Ascension
{
    public class JobDriver_FoundationTraining : JobDriver
    {
        private const int BaseDurationTicks = 2500; // 1 hour
        public const TargetIndex SpotInd = TargetIndex.B;

        //does this part afte time calculations not before
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (job.GetTarget(SpotInd) != pawn)
            {
                return pawn.MapHeld.reservationManager.Reserve(pawn, job, job.GetTarget(SpotInd), 1, -1, null, errorOnFailed);
            }
            else
            {
                return true;
            }
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);

            Toil waitToil = Toils_Cultivation.Wait(BaseDurationTicks, AscensionDefOf.AS_FoundationTrainingJob).WithProgressBarToilDelay(TargetIndex.A);

            Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, waitToil, AscensionDefOf.AS_FoundationTrainingJob);
            yield return calculateDurationToil;

            yield return waitToil;
            yield return Toils_General.Do(Exercise);
        }

        private void Exercise()
        {
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                cultivatorHediff.foundationTrainingJobProg = 0;
                AscensionUtilities.FoundationProgress(pawn, 10);
            }
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
