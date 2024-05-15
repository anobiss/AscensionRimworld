using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class IncidentWorker_SpiritTreeConversion : IncidentWorker
    {

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            Map map = (Map)parms.target;
            IntVec3 result;
            if (AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, map, out var _))
            {
                return RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal);
            }
            return false;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;


            foreach (Thing thing in map.listerThings.AllThings)
            {

            }


            PawnKindDef animalKind = parms.pawnKind;
            if ((animalKind == null && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, map, out animalKind)) || AggressiveAnimalIncidentUtility.GetAnimalsCount(animalKind, parms.points) == 0)
            {
                return false;
            }
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal))
            {
                return false;
            }
            List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, map.Tile, parms.points * 1f, parms.pawnCount);
            Rot4 rot = Rot4.FromAngleFlat((map.Center - result).AngleFlat);
            for (int i = 0; i < list.Count; i++)
            {
                Pawn pawn = list[i];
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, map, 10);
                QuestUtility.AddQuestTag(GenSpawn.Spawn(pawn, loc, map, rot), parms.questTag);
                pawn.health.AddHediff(HediffDefOf.Scaria);
                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
                pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
            }
            if (ModsConfig.AnomalyActive)
            {
                if (def == IncidentDefOf.FrenziedAnimals)
                {
                    SendStandardLetter("FrenziedAnimalsLabel".Translate(), "FrenziedAnimalsText".Translate(animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, list[0]);
                }
                else
                {
                    SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, list[0]);
                }
            }
            else
            {
                SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, list[0]);
            }
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
            return true;
        }
    }
}
