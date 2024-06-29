using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace Ascension
{
    public class JobDriver_RefineQi : JobDriver
    {
        private const int BaseDurationTicks = 10000; // 4 hours
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
            yield return Toils_General.Do(RefineQi);
        }

        private void RefineQi()
        {
            //Log.Message("refining qi");
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool, false) as QiPool_Hediff;
            Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
            if (qiPool != null && essenceHediff != null)
            {
                float qiCost = 2 + (qiPool.maxAmount / 10); // 10% + 2
                if (qiPool.amount >= qiCost)
                {
                    //Log.Message("increasing progression by " + qiCost);
                    AscensionUtilities.TierProgress(pawn, AscensionDefOf.EssenceRealm, qiCost);
                    qiPool.amount -= qiCost;
                }
                else
                {
                    //Log.Message("cost too high");
                }
            }
            else
            {
                //Log.Message("no qipool or essence realm");
            }
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
