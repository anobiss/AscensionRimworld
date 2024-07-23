using RimWorld;
using Verse;
using Verse.Noise;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Ascension
{
    public class AS_CompAbilityEffect_ThingCircle : CompAbilityEffect
    {
        public new AS_CompProperties_ThingCircle Props => (AS_CompProperties_ThingCircle)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            CreateThingCircle(target);
        }

        private void CreateThingCircle(LocalTargetInfo target)
        {
            if (parent.pawn.Map != null)
            {
                Map map = parent.pawn.Map;
                IntVec3 center = target.Cell;
                float innerRadius = Props.innerCircleRadius;
                float outerRadius = Props.outerCircleRadius;

                List<IntVec3> cellsInCircle = GenRadial.RadialCellsAround(center, outerRadius, true).ToList();
                cellsInCircle.RemoveAll(cell => cell.DistanceTo(center) < innerRadius || !cell.InBounds(map));

                foreach (IntVec3 cell in cellsInCircle)
                {
                    if (cell.Walkable(map))
                    {
                        Thing circleThing = ThingMaker.MakeThing(Props.circleThing);
                        GenPlace.TryPlaceThing(circleThing, cell, map, ThingPlaceMode.Near);
                    }
                }
            }
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
