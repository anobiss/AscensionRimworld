
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
        private const int DurationTicks = 7500;//3 hours
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
            //yes
            int cultivationJobTicks = DurationTicks;
            //Log.Message("initial cultivationJobTicks" + cultivationJobTicks);
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                //log important only for testing
                float cultivationSpeed = AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                //Log.Message("current cultivation speed is" + cultivationSpeed);

                float cultivationTicks = DurationTicks/AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                //Log.Message("cultivation ticks float is" + cultivationTicks);
                cultivationJobTicks = (int)Math.Floor(cultivationTicks);
                //Log.Message("ticks now cultivationJobTicks"+ cultivationJobTicks);
            }
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
            yield return Toils_General.Wait(cultivationJobTicks).WithProgressBarToilDelay(TargetIndex.A);
            yield return Toils_General.Do(QiGather);
        }

        private void QiGather()
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;

            QiGatherMapComponent qiGatherMapComp = pawn.Map.GetComponent<QiGatherMapComponent>();
            int qiAmount = qiGatherMapComp.GetQiGatherAt(pawn.Position.x, pawn.Position.y)+1;
            AscensionUtilities.IncreaseQi(pawn, qiAmount, true);
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
