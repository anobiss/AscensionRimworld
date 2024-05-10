
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using RimWorld;
using static HarmonyLib.Code;
using static UnityEngine.GraphicsBuffer;
using System.Reflection.Emit;
using System.Security.Principal;

namespace Ascension
{
    public class JobDriver_RefineQiCauldron : JobDriver
    {
        private const int DurationTicks = 15000;//6 hours
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
            yield return Toils_General.Do(RefineQiCauldron);
        }
        private void RefineQiCauldron()
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool, false) as QiPool_Hediff;
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (qiPool != null && cultivatorHediff != null)
            {
                int qiCost = 2 + (qiPool.maxAmount / 50);// 2% + 2
                if (qiPool.amount >= qiCost)
                {
                    if (cultivatorHediff.innerCauldronQi < cultivatorHediff.innerCauldronLimit) // this is so we dont get the breakthroughpossible message over and over again
                    {
                        AscensionUtilities.CauldronIncrease(pawn, qiCost);
                        qiPool.amount -= qiCost;
                    }
                }
            }
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}
