using RimWorld;
using Verse;

namespace Ascension
{
    public class AS_CompAbilityEffect_AbilityLifespanCost : CompAbilityEffect
    {
        public new AS_CompProperties_AbilityLifespanCost Props => (AS_CompProperties_AbilityLifespanCost)props;
        private bool HasEnoughLifespan
        {
            get
            {
                bool enoughLifespan = false;
                Cultivator_Hediff cultivatorHediff = parent.pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                if (cultivatorHediff != null)
                {
                    if (cultivatorHediff.lifespan >= Props.cost)
                    {
                        enoughLifespan = true;
                    } 
                }
                return enoughLifespan;
            }
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Cultivator_Hediff cultivatorHediff = parent.pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                cultivatorHediff.lifespan -= Props.cost;
            }
        }
        public override bool GizmoDisabled(out string reason)
        {
            HediffSet hediffs = parent.pawn.health.hediffSet;
            Cultivator_Hediff cultivatorHediff = hediffs.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff == null)
            {
                reason = "AS_AbilityNotCultivator".Translate(parent.pawn);
                return true;
            }
            if (cultivatorHediff.lifespan < Props.cost)
            {
                reason = "AS_AbilityDisabledNoLifespan".Translate(parent.pawn);
                return true;
            }
            reason = null;
            return false;
        }
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return HasEnoughLifespan;
        }
    }
}
