
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Ascension
{
    public class JobDriver_RefineQiCauldron : JobDriver
    {
        private const int BaseDurationTicks = 15000;//6 hours
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
            yield return Toils_General.Do(RefineQiCauldron);
        }

        private void RefineQiCauldron()
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool, false) as QiPool_Hediff;
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (qiPool != null && cultivatorHediff != null)
            {
                float qiCost = 2f + (qiPool.maxAmount / 50f);// 2% + 2
                if (qiPool.amount >= qiCost)
                {
                    AscensionUtilities.CauldronIncrease(pawn, qiCost);
                    qiPool.amount -= qiCost;
                }
            }
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
