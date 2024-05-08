
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using RimWorld;
using static HarmonyLib.Code;

namespace Ascension
{
    public class JobDriver_RefineQi : JobDriver
    {
        private const int DurationTicks = 10000;//4 hours
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
            yield return Toils_General.Do(RefineQi);
        }
        private void RefineQi()
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool, false) as QiPool_Hediff;
            Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
            if (qiPool != null && essenceHediff != null)
            {
                int qiCost = 2 + (qiPool.maxAmount / 10);// 10% + 2
                if (qiPool.amount >= qiCost)
                {
                    if (essenceHediff.progress < essenceHediff.maxProgress) // this is so we dont get the breakthroughpossible message over and over again
                    {
                        AscensionUtilities.TierProgress(pawn, AscensionDefOf.EssenceRealm, qiCost);
                        qiPool.amount -= qiCost;
                    }
                }
            }
        }
    }
}
