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
        private const int DurationTicks = 17500; // 7 hours
        private const int QiConsumptionPerCycle = 25;
        public const TargetIndex SpotInd = TargetIndex.B;

        //does this part after time calculations not before
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
            int cultivationJobTicks = DurationTicks;
            int cultivationTicksCounter = 0;

            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;

            if (cultivatorHediff != null && qiPool != null)
            {
                float cultivationSpeed = AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                float cultivationTicks = DurationTicks / cultivationSpeed;
                cultivationJobTicks = (int)Math.Floor(cultivationTicks);

                yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);

                Toil cultivationToil = new Toil();
                cultivationToil.initAction = () =>
                {
                    cultivationToil.actor.pather.StopDead();
                };
                cultivationToil.tickAction = () =>
                {
                    if (cultivationTicksCounter >= cultivationJobTicks)
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
                cultivationToil.defaultCompleteMode = ToilCompleteMode.Never;
                cultivationToil.AddPreTickAction(() =>
                {
                    if (qiPool.amount < QiConsumptionPerCycle)
                    {
                        Breakthrough(currentScore);
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                });

                yield return cultivationToil.WithProgressBar(TargetIndex.A, () => (float)cultivationTicksCounter / cultivationJobTicks, true);

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
