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
    public class JobDriver_BreakthroughEssence : JobDriver
    {
        private const int BaseDurationTicks = 17500; // 7 hours
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
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);

            Toil waitToil = Toils_General.Wait(BaseDurationTicks).WithProgressBarToilDelay(TargetIndex.A);

            Toil calculateDurationToil = Toils_Cultivation.CalculateDuration(BaseDurationTicks, waitToil);
            yield return calculateDurationToil;

            yield return waitToil;
            yield return Toils_General.Do(Breakthrough);
        }

        private void Breakthrough()
        {
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                float breakthroughChance = AscensionUtilities.UpdateBreakthroughChance(cultivatorHediff);
                if (Rand.Value < breakthroughChance)
                {
                    AscensionUtilities.TierBreakthrough((Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm));
                }
            }
            if (job.GetTarget(SpotInd) != pawn)
            {
                pawn.MapHeld.reservationManager.Release(job.GetTarget(SpotInd), pawn, job);
            }
        }
    }
}




//        --
//        float newWarmupTime = ability.def.verbProperties.warmupTime;

//        HediffSet hediffSet = ability.pawn.health.hediffSet;
//        Hediff hediff;

//            if (hediffSet.HasHediff(AscensionDefOf.PseudoImmortality))
//            {
//                hediff = hediffSet.GetFirstHediffOfDef(AscensionDefOf.PseudoImmortality, false);
//                newWarmupTime += hediff.Severity* 42; // 1 hour per tier
//            }
//            else if (hediffSet.HasHediff(AscensionDefOf.AscendantFoundation))
//            {
//                hediff = hediffSet.GetFirstHediffOfDef(AscensionDefOf.PseudoImmortality, false);
//                newWarmupTime += hediff.Severity* 126; // 3 hours per tier
//            }
//bool num = base.TryStartCastOn(castTarg, destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
//if (num && ability.def.stunTargetWhileCasting && newWarmupTime > 0f && castTarg.Thing is Pawn pawn && pawn != ability.pawn)
//{
//    pawn.stances.stunner.StunFor(newWarmupTime.SecondsToTicks(), ability.pawn, addBattleLog: false, showMote: false);
//    if (!pawn.Awake())
//    {
//        RestUtility.WakeUp(pawn);
//    }
//}
//return num;

//--

