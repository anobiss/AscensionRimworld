using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Ascension
{
    public class CompAssignableToPawn_CultivationSpot : CompAssignableToPawn
    {
        private CultivationOwnershipMapComponent ownershipMapComp;
        public override IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                if (!parent.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                List<Pawn> cultivatorPawns = new List<Pawn>(); ;//reset list for new one
                foreach (Pawn pawn in parent.Map.mapPawns.FreeColonists)//check colonist cultivators
                {
                    Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                    if (cultivatorHediff != null)
                    {
                        cultivatorPawns.Add(pawn);
                    }
                }
                foreach (Pawn pawn in parent.Map.mapPawns.SpawnedColonyAnimals)//check animal cultivators
                {
                    Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                    if (cultivatorHediff != null)
                    {
                        cultivatorPawns.Add(pawn);
                    }
                }
                return cultivatorPawns.OrderByDescending((Pawn p) => CanAssignTo(p).Accepted);
            }
        }

        protected override string GetAssignmentGizmoDesc()
        {
            return "AS_CommandCultivationSpotSetOwnerDesc".Translate();
        }

        public override string CompInspectStringExtra()
        {
            if (base.AssignedPawnsForReading.Count == 0)
            {
                return "Owner".Translate() + ": " + "Nobody".Translate();
            }
            if (base.AssignedPawnsForReading.Count == 1)
            {
                return "Owner".Translate() + ": " + base.AssignedPawnsForReading[0].Label;
            }
            return "";
        }

        public override bool AssignedAnything(Pawn pawn)
        {
            ownershipMapComp = pawn.Map.GetComponent<CultivationOwnershipMapComponent>();
            if (ownershipMapComp != null)
            {
                return ownershipMapComp.GetBuildingAssignedToPawn(pawn) != null;// means its assigned to something
            }
            return false;// no map comp no assignments
        }

        public override void TryAssignPawn(Pawn pawn)
        {
            ownershipMapComp = pawn.Map.GetComponent<CultivationOwnershipMapComponent>();
            if (ownershipMapComp != null)
            {
                ownershipMapComp.ClaimCultivationSpot( pawn, (Building)parent);
            }
        }

        public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
        {
            ownershipMapComp = pawn.Map.GetComponent<CultivationOwnershipMapComponent>();
            if (ownershipMapComp != null)
            {
                ownershipMapComp.UnclaimCultivationSpot(pawn);
            }
        }




        public override void PostExposeData()
        {
            base.PostExposeData();

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (assignedPawns.RemoveAll((Pawn x) =>
                {
                    var comp = x.Map.GetComponent<CultivationOwnershipMapComponent>();
                    return comp == null || comp.GetBuildingAssignedToPawn(x) != parent;
                }) > 0)
                {
                    Log.Warning(parent.ToStringSafe() + " had pawns assigned that don't have it as an assigned cultivation spot. Removing.");
                }
            }
        }
    }
}