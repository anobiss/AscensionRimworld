

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

            Toil waitToil = Toils_General.Wait(BaseDurationTicks).WithProgressBarToilDelay(TargetIndex.A);

            Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, waitToil);
            yield return calculateDurationToil;

            yield return waitToil;
            yield return Toils_General.Do(Exercise);
        }

        private void Exercise()
        {
            float maxBody = 10;//default always given amount
            Realm_Hediff bodyHediff = (Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm);
            if (bodyHediff != null)
            {
                maxBody += bodyHediff.maxProgress / 100;//for adding 1 percent body to amount
            }
            AscensionUtilities.TierProgress(pawn, AscensionDefOf.BodyRealm, maxBody);
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
