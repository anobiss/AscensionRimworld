
using RimWorld;
using System;
using UnityEngine;
using Verse;
using Random = System.Random;

namespace Ascension
{
    public class CompAbilityEffect_HediffToggle : CompAbilityEffect
    {
        public new CompProperties_AbilityHediffToggle Props => (CompProperties_AbilityHediffToggle)props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = parent.pawn;
            if (pawn == null)
            {
                return;
            }
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }else
            {
                pawn.health.AddHediff(this.Props.hediffDef);
            }
            FleckMaker.AttachedOverlay(pawn, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
        }
    }
}
