using RimWorld;
using System;
using UnityEngine;
using Verse;
using Random = System.Random;

namespace Ascension
{
    public class CompAbilityEffect_HediffGiver : CompAbilityEffect
    {
        private float RandomSeverity(float min, float max)
        {
            Random r = new Random();
            float result = 0.01f * (float)(r.Next(Mathf.RoundToInt(min * 100f), Mathf.RoundToInt(max * 100f)));
            return result;
        }
        public new CompProperties_AbilityHediffGiver Props => (CompProperties_AbilityHediffGiver)props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = parent.pawn;
            if (pawn == null)
            {
                return;
            }
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef);
            if (AscensionDefOf.QiPool == this.Props.hediffDef)//checks if qi pool
            {
                AscensionUtilities.IncreaseQi(pawn, (int)Math.Floor(100 * RandomSeverity(this.Props.minSeverity, this.Props.maxSeverity)));
            }else// do normal stuff if not qi
            {
                if (hediff != null)
                {
                    hediff.Severity += RandomSeverity(this.Props.minSeverity, this.Props.maxSeverity);
                }
                else
                {
                    Hediff hediff2 = HediffMaker.MakeHediff(this.Props.hediffDef, pawn, null);
                    hediff2.Severity = RandomSeverity(this.Props.minSeverity, this.Props.maxSeverity);
                    pawn.health.AddHediff(hediff2, null, null, null);
                }
            }

            FleckMaker.AttachedOverlay(pawn, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
        }
    }
}
