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
    public class JobDriver_GoToCulivationSpot : JobDriver
    {
        private const int DurationTicks = 7500;//3 hours

        public const TargetIndex SpotInd = TargetIndex.B;

        //does this part afte time calculations not before
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            //checks if theres an unoccupied cultivation spot and goes to it if there is
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
            yield return Toils_General.Do(CultivationJob);
        }

        private void CultivationJob()
        {
            pawn.jobs.StartJob(CultivationJobUtility.GetCultivationJob(pawn));
        }
    }
}
