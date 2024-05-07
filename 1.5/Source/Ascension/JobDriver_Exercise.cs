

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
        private const int DurationTicks = 10000; // 4 hours
        public const TargetIndex SpotInd = TargetIndex.B;

        //does this part afte time calculations not before
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            int cultivationJobTicks = DurationTicks;
            HediffDef cultivatorDef = AscensionDefOf.Cultivator;
            if (pawn.health.hediffSet.HasHediff(cultivatorDef))
            {
                Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(cultivatorDef) as Cultivator_Hediff;
                AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                cultivationJobTicks = (int)Math.Floor(DurationTicks / cultivatorHediff.cultivationSpeed);
            }
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
            yield return Toils_General.Wait(cultivationJobTicks).WithProgressBarToilDelay(TargetIndex.A);
            yield return Toils_General.Do(Exercise);
        }

        private void Exercise()
        {
            int maxBody = 10;//default always given amount
            Realm_Hediff bodyHediff = (Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm);
            if (bodyHediff != null)
            {
                maxBody += bodyHediff.maxProgress / 100;//for adding 1 percent body to amount
            }
            AscensionUtilities.TierProgress(pawn, AscensionDefOf.BodyRealm, maxBody);
        }
    }
}
