using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class CompAbilityEffect_QiHeal : CompAbilityEffect
    {
        public new CompProperties_AbilityQiCost Props => (CompProperties_AbilityQiCost)props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = target.Pawn;
            if (pawn == null)
            {
                return;
            }
            AscensionUtilities.PreHeal(pawn);
            HealthUtility.FixWorstHealthCondition(pawn);
            FleckMaker.AttachedOverlay(pawn, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
        }
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Pawn pawn = target.Pawn;
            if (pawn != null)
            {
                List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                for (int i = 0; i < hediffs.Count; i++)
                {
                    if (hediffs[i] is Hediff_Injury || hediffs[i] is Hediff_MissingPart)
                    {
                        return true;
                    }
                }
            }
            return base.Valid(target, throwMessages);
        }
    }
}
