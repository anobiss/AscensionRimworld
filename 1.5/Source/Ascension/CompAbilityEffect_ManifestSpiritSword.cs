using RimWorld;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class CompAbilityEffect_ManifestSpiritSword : CompAbilityEffect
    {
        public new CompProperties_AbilityManifestSpiritSword Props => (CompProperties_AbilityManifestSpiritSword)props;
        public static void SpiritSwordSpawn(Pawn pawn, IntVec3 cell)
        {
            //Log.Message($"Making thing Spirit Sword at {pawn.LabelShort} in {pawn.InteractionCell}.");
            Thing thing = ThingMaker.MakeThing(AscensionDefOf.SpiritSword, null);
            thing.stackCount = 1;
            GenPlace.TryPlaceThing(thing, pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, out Thing t, null, null, default(Rot4));
            //Log.Message($"Removing Spirit Sword abilities and hediffs of {pawn.LabelShort}");
            pawn.abilities.RemoveAbility(AscensionDefOf.ManifestSpiritSword);
            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.SpiritSwordFusion));
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = parent.pawn;
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.SpiritSwordFusion);
            if (pawn == null || hediff == null) { return; }
            //Log.Message($"Attempting to spawn Spirit Sword at {pawn.LabelShort} in {pawn.InteractionCell}...");
            SpiritSwordSpawn(pawn, dest.Cell);
            FleckMaker.AttachedOverlay(pawn, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
        }
    }
}
