using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace Ascension
{
    public class JobDriver_RefineQi : JobDriver
    {
        private const int BaseDurationTicks = 10000; // 4 hours
        public const TargetIndex SpotInd = TargetIndex.B;
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

            Toil waitToil = Toils_Cultivation.Wait(BaseDurationTicks, AscensionDefOf.AS_RefineQiJob).WithProgressBarToilDelay(TargetIndex.A);
            //when wait Toil fails we want to save progress for future attempts and reset saved progress when we the Toil sucseeds/ is not inturupted

            Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, waitToil, AscensionDefOf.AS_RefineQiJob);
            yield return calculateDurationToil;

            yield return waitToil;
            yield return Toils_General.Do(RefineQi);
        }

        private void RefineQi()
        {
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                cultivatorHediff.refineQiJobProg = 0;
            }
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool, false) as QiPool_Hediff;
            Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
            if (qiPool != null && essenceHediff != null)
            {
                float qiCost = 2 + (qiPool.maxAmount / 10); // 10% + 2
                if (qiPool.amount >= qiCost)
                {
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
            FleckMaker.AttachedOverlay(pawn, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
            MoteMaker.MakeAttachedOverlay(pawn, AscensionDefOf.Mote_QiMistA, Vector3.zero, 1.5f, -1f);
        }
    }
}
