
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using RimWorld;

namespace Ascension
{
    public class JobDriver_QiGathering : JobDriver
    {
        private const int BaseDurationTicks = 7500;//3 hours

        public const TargetIndex SpotInd = TargetIndex.B;

        // Reserve the spot after time calculations
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

            Toil waitToil = Toils_General.Wait(BaseDurationTicks).WithProgressBarToilDelay(TargetIndex.A);

            Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, waitToil);
            yield return calculateDurationToil;

            yield return waitToil;
            yield return Toils_General.Do(QiGather);
        }

        private void QiGather()
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;

            QiGatherMapComponent qiGatherMapComp = pawn.Map.GetComponent<QiGatherMapComponent>();
            int qiAmount = qiGatherMapComp.GetQiGatherAt(pawn.Position.x, pawn.Position.z)+1;
            AscensionUtilities.IncreaseQi(pawn, qiAmount, true);
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
