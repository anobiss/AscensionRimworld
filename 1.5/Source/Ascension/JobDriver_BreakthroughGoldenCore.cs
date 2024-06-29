using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace Ascension
{
    //Golden Core breakthroughs work entirely different from every other.
    //100 max qi per (hour divided by cultivation speed)
    //finishes when interrupted or ended by running out of qi
    //qi recovery is turned off during this breakthrough
    public class JobDriver_GoldenCoreBreakthrough : JobDriver
    {
        private const int BaseDurationTicks = 17500; // 7 hours
        private const int QiConsumptionPerCycle = 25;
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
            int currentScore = 0;
            int cultivationTicksCounter = 0;
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;

            if (qiPool != null)
            {
                yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);

                Toil cWaitToil = new Toil();
                Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, cWaitToil);
                yield return calculateDurationToil;

                cWaitToil.initAction = () =>
                {
                    cWaitToil.actor.pather.StopDead();
                };
                cWaitToil.tickAction = () =>
                {
                    if (cultivationTicksCounter >= cWaitToil.defaultDuration)
                    {
                        cultivationTicksCounter = 0;

                        if (qiPool.amount >= QiConsumptionPerCycle)
                        {
                            qiPool.amount -= QiConsumptionPerCycle;
                            currentScore += QiConsumptionPerCycle;
                        }
                        else
                        {
                            Breakthrough(currentScore);
                            this.EndJobWith(JobCondition.Succeeded);
                        }
                    }
                    cultivationTicksCounter++;
                };
                cWaitToil.defaultCompleteMode = ToilCompleteMode.Never;
                cWaitToil.AddPreTickAction(() =>
                {
                    if (qiPool.amount < QiConsumptionPerCycle)
                    {
                        Breakthrough(currentScore);
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                });
                cWaitToil.WithProgressBar(TargetIndex.A, () => (float)cultivationTicksCounter / cWaitToil.defaultDuration, true);

                yield return cWaitToil;
                yield return Toils_General.Do(() => Breakthrough(currentScore));
            }
        }

        private void Breakthrough(int score)
        {
            AscensionUtilities.GoldenCoreBreakthrough(pawn, score);
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
