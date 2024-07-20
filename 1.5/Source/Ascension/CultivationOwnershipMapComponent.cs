using Verse;
using System.Collections.Generic;
using RimWorld;
using System.Linq;

namespace Ascension
{
    public class CultivationOwnershipMapComponent : MapComponent
    {
        private Dictionary<Pawn, Building> pawnToBuildingMap;

        public CultivationOwnershipMapComponent(Map map) : base(map)
        {
            pawnToBuildingMap = new Dictionary<Pawn, Building>();
        }

        public void AssignPawnToBuilding(Pawn pawn, Building building)
        {
            if (pawn == null || building == null)
            {
                Log.Error("Cannot assign null pawn or building.");
                return;
            }

            if (pawnToBuildingMap.ContainsKey(pawn))
            {
                pawnToBuildingMap.Remove(pawn);
            }

            pawnToBuildingMap[pawn] = building;
        }

        public Building GetBuildingAssignedToPawn(Pawn pawn)
        {
            if (pawnToBuildingMap.TryGetValue(pawn, out Building building))
            {
                return building;
            }
            return null;
        }

        public void RemovePawnAssignment(Pawn pawn)
        {
            if (pawnToBuildingMap.ContainsKey(pawn))
            {
                pawnToBuildingMap.Remove(pawn);
            }
        }

        public void ClearAllAssignments()
        {
            pawnToBuildingMap.Clear();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref pawnToBuildingMap, "pawnToBuildingMap", LookMode.Reference, LookMode.Reference);
        }

        // Methods for claiming and unclaiming cultivation spots

        public bool ClaimCultivationSpot(Pawn pawn, Building newSpot)
        {
            if (pawn == null || newSpot == null)
            {
                Log.Error("Cannot claim a spot with null pawn or building.");
                return false;
            }

            // Unclaim the current cultivation spot if any
            Building currentSpot = GetBuildingAssignedToPawn(pawn);
            if (currentSpot != null)
            {
                currentSpot.TryGetComp<CompAssignableToPawn>().ForceRemovePawn(pawn);
                RemovePawnAssignment(pawn);
            }

            // Get the CompAssignableToPawn for the new spot
            var compAssignable = newSpot.TryGetComp<CompAssignableToPawn>();
            if (compAssignable == null)
            {
                Log.Error("The new spot does not have a CompAssignableToPawn component.");
                return false;
            }

            // Clear ownership of the spot from everyone else
            foreach (var assignedPawn in compAssignable.AssignedPawns.ToList())
            {
                compAssignable.ForceRemovePawn(assignedPawn);
                RemovePawnAssignment(assignedPawn);
            }

            // Assign the new spot
            if (compAssignable.AssignedPawns.Contains(pawn))
            {
                return false; // Pawn is already assigned to this spot
            }

            compAssignable.ForceAddPawn(pawn);
            AssignPawnToBuilding(pawn, newSpot);  // Store the new spot
            return true;
        }

        public bool UnclaimCultivationSpot(Pawn pawn)
        {
            // Get and unassign the current cultivation spot
            Building currentSpot = GetBuildingAssignedToPawn(pawn);
            if (currentSpot == null)
            {
                return false;
            }

            currentSpot.TryGetComp<CompAssignableToPawn>().ForceRemovePawn(pawn);
            RemovePawnAssignment(pawn);
            return true;
        }
    }
}
