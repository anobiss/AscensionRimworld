

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
            //Log.Message("initial cultivationJobTicks" + cultivationJobTicks);
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                //log important only for testing
                float cultivationSpeed = AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                //Log.Message("current cultivation speed is" + cultivationSpeed);

                float cultivationTicks = DurationTicks / AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                //Log.Message("cultivation ticks float is" + cultivationTicks);
                cultivationJobTicks = (int)Math.Floor(cultivationTicks);
                //Log.Message("ticks now cultivationJobTicks"+ cultivationJobTicks);
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
