using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ascension
{
    public class AS_CompAbilityEffect_GrowPlants : CompAbilityEffect
    {
        public new AS_CompProperties_GrowPlants Props => (AS_CompProperties_GrowPlants)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            GrowPlantsInCircle(target);
        }

        private void GrowPlantsInCircle(LocalTargetInfo target)
        {
            if (target.Cell == null || parent.pawn.Map == null)
            {
                Log.Error("AS_CompAbilityEffect_GrowPlants: Target or target map is null.");
                return;
            }

            Map map = parent.pawn.Map;
            IntVec3 center = target.Cell;
            float radius = Props.growRadius;
            float growDays = Props.growDays;

            // Get all cells in the specified radius
            List<IntVec3> cellsInCircle = GenRadial.RadialCellsAround(center, radius, true).ToList();
            cellsInCircle.RemoveAll(cell => !cell.InBounds(map));

            // Iterate over each cell to grow plants
            foreach (IntVec3 cell in cellsInCircle)
            {
                Plant plant = cell.GetPlant(map);
                if (plant != null && plant.def.plant != null)
                {
                    int growthTicks = (int)((1f - plant.Growth) * growDays * 60000f); // Convert growDays to ticks
                    plant.Age += growthTicks;
                    plant.Growth = 1f;
                    FleckMaker.AttachedOverlay(plant, AscensionDefOf.FlashQi, Vector3.zero, 1.5f, -1f);
                    map.mapDrawer.SectionAt(cell).RegenerateAllLayers();
                }
            }
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
