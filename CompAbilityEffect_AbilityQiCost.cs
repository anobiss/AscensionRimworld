using RimWorld;
using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace Ascension
{

    public class CompAbilityEffect_AbilityQiCost : CompAbilityEffect
    {
        public new CompProperties_AbilityQiCost Props => (CompProperties_AbilityQiCost)props;

        private bool HasEnoughQi
        {
            get
            {
                HediffSet hediffs = this.parent.pawn.health.hediffSet;
                QiPool_Hediff hediff_QiPool = (hediffs != null) ? (QiPool_Hediff)hediffs.GetFirstHediffOfDef(AscensionDefOf.QiPool) : null;

                return hediff_QiPool != null && hediff_QiPool.amount >= this.Props.cost;
            }


        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            QiPool_Hediff hediff = (QiPool_Hediff)parent.pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool);
            hediff.amount = hediff.amount - this.Props.cost;
        }

        public override void PostApplied(List<LocalTargetInfo> targets, Map map)
        {
            if (Props.removeHediffAfterCasting)
            {
                Hediff hediff = parent.pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool);
                if (hediff != null)
                {
                    parent.pawn.health.RemoveHediff(hediff);
                }
            }
        }

        public override bool GizmoDisabled(out string reason)
        {
            HediffSet hediffs = this.parent.pawn.health.hediffSet;
            QiPool_Hediff hediff_QiPool = (hediffs != null) ? (QiPool_Hediff)hediffs.GetFirstHediffOfDef(AscensionDefOf.QiPool) : null;
            if (hediff_QiPool == null)
            {
                reason = "AbilityDisabledNoQiPool".Translate(this.parent.pawn);
                //Log.Message($"No Qi pool found on {this.parent.pawn.LabelShort}");
                return true;
            }
            if (hediff_QiPool.amount < this.Props.cost)
            {
                reason = "AbilityDisabledNoQi".Translate(this.parent.pawn);
                //Log.Message($"Not enough Qi for this ability on {this.parent.pawn.LabelShort} {hediff_QiPool.Severity} severity with cost of {this.Props.QiCost}");
                return true;
            }

            if (this.Props.reqHediffDef != null)
            {
                if (hediffs.GetFirstHediffOfDef(this.Props.reqHediffDef) == null)
                {
                    reason = "AbilityDisabledNoHediff".Translate(this.parent.pawn)+ this.Props.reqHediffDef.label;
                    //Log.Message("Not right hediffoe");
                    return true;
                }
            }
            //this part is for the future people to worry about

            //float num = this.TotalQiCostOfQueuedAbilities();
            //float num2 = this.Props.QiCost + num;
            //if (this.Props.QiCost > 1E-45f && num2 > hediff_QiPool.Severity)
            //{
            //    reason = "AbilityDisabledNoQiQued".Translate(this.parent.pawn);
            //    Log.Message($"Not enough Qi for Qued abilities on {this.parent.pawn.LabelShort} {hediff_QiPool.Severity} severity with total cost of {num2}");
            //    return true;
            //}
            reason = null;
            return false;
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return this.HasEnoughQi;
        }


        public float TotalQiCostOfQueuedAbilities()
        {
            float QiTotalCost = 0f;

            List<Ability> abilityList = this.parent.pawn.abilities.AllAbilitiesForReading;
            foreach (Ability ability in abilityList)
            {
                foreach (CompAbilityEffect cae in ability.EffectComps)
                {
                    if (cae is CompAbilityEffect_AbilityQiCost Qi)
                    {
                        QiTotalCost += Qi.Props.cost; // whatever you need to access
                    }
                }
            }
            return QiTotalCost;
        }

    }
}
