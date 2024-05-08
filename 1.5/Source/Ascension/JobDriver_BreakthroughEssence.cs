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
        private const int DurationTicks = 17500; // 7 hours
        public const TargetIndex SpotInd = TargetIndex.B;

        //does this part after time calculations not before
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
            yield return Toils_General.Do(Breakthrough);
        }

        private void Breakthrough()
        {
            AscensionUtilities.TierBreakthrough((Realm_Hediff)pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm));
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

