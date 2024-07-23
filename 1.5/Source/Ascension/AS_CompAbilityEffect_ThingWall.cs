using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace Ascension
{
    public class AS_CompAbilityEffect_ThingWall : CompAbilityEffect
    {
        public new AS_CompProperties_ThingWall Props => (AS_CompProperties_ThingWall)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            CreateWall(target);
        }

        private void CreateWall(LocalTargetInfo target)
        {
            if (parent.pawn.Map != null)
            {
                Map map = parent.pawn.Map;
                IntVec3 center = target.Cell;
                IntVec3 casterPos = parent.pawn.Position;
                ThingDef wallThing = Props.wallThing;

                // wall orientation based on position
                bool isHorizontal = Mathf.Abs(center.x - casterPos.x) > Mathf.Abs(center.z - casterPos.z);

                List<IntVec3> wallCells = new List<IntVec3>();
                if (isHorizontal)
                {
                    for (int i = -Props.wallLWidth / 2; i <= Props.wallLWidth / 2; i++)
                    {
                        for (int j = 0; j < Props.wallHieght; j++)
                        {
                            wallCells.Add(new IntVec3(center.x + i, 0, center.z + j));
                        }
                    }
                }
                else
                {
                    for (int i = -Props.wallLWidth / 2; i <= Props.wallLWidth / 2; i++)
                    {
                        for (int j = 0; j < Props.wallHieght; j++)
                        {
                            wallCells.Add(new IntVec3(center.x + j, 0, center.z + i));
                        }
                    }
                }

                // wallinate the things
                foreach (IntVec3 cell in wallCells)
                {
                    if (cell.InBounds(map) && cell.Walkable(map))
                    {
                        Thing wall = ThingMaker.MakeThing(wallThing);
                        GenPlace.TryPlaceThing(wall, cell, map, ThingPlaceMode.Near);
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
