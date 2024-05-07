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
    public class JobDriver_BreakthroughBody : JobDriver
    {
        private const int DurationTicks = 17500; // 7 hours
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
            yield return Toils_General.Do(Breakthrough);
        }

        private void Breakthrough()
        {
            AscensionUtilities.TierBreakthrough((Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm));
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

