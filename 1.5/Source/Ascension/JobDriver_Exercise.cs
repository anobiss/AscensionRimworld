

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
    public class JobDriver_Exercise : JobDriver
    {
        private const int BaseDurationTicks = 10000; // 4 hours
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

            Toil waitToil = Toils_Cultivation.Wait(BaseDurationTicks, AscensionDefOf.AS_ExerciseJob).WithProgressBarToilDelay(TargetIndex.A);

            Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, waitToil, AscensionDefOf.AS_ExerciseJob);
            yield return calculateDurationToil;

            yield return waitToil;
            yield return Toils_General.Do(Exercise);
        }

        private void Exercise()
        {
            float progressBody = 1;//default always given amount
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            Realm_Hediff bodyHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
            ElementEmitMapComponent elementEmitMapComp = pawn.Map.GetComponent<ElementEmitMapComponent>();

            if (cultivatorHediff != null)
            {
                cultivatorHediff.exerciseJobProg = 0;
                if (bodyHediff != null)
                {
                    if (elementEmitMapComp != null)
                    {
                        progressBody += (elementEmitMapComp.CalculateElementValueAt(new IntVec2(pawn.Position.x, pawn.Position.z), cultivatorHediff.element)/10);
                    }
                    progressBody += bodyHediff.maxProgress / 100;//for adding 1 percent body to amount
                }
            }


            AscensionUtilities.TierProgress(pawn, AscensionDefOf.BodyRealm, progressBody);
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
