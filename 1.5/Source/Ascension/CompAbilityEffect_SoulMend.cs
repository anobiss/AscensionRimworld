
using RimWorld;
using System;
using UnityEngine;
using Verse;
using Random = System.Random;

namespace Ascension
{
    public class CompAbilityEffect_SoulMend : CompAbilityEffect
    {
        public new CompProperties_AbilitySoulMend Props => (CompProperties_AbilitySoulMend)props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {

            base.Apply(target, dest);
            Pawn pawn = parent.pawn;
            SoulMend_Hediff soulMendHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.AS_SoulMend) as SoulMend_Hediff;
            if (pawn == null)
            {
                return;
            }

            if  (soulMendHediff == null)
            {
                soulMendHediff = HediffMaker.MakeHediff(AscensionDefOf.AS_SoulMend, pawn, null) as SoulMend_Hediff;
                soulMendHediff.Severity = 1f;
                pawn.health.AddHediff(soulMendHediff, null, null, null);
            }else
            {
                soulMendHediff.Severity += 1f;
            }

            FleckMaker.AttachedOverlay(pawn, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
        }
    }
}
