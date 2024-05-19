using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Ascension
{
    public class CultivationJobUtility
    {
        public static Thing FindCultivationSpot(Pawn pawn)//returns the cultivation spot a pawn should be using, if none returns the pawn itself.
        {
            Thing cultivationSpotThing = pawn;//if no cultivation spot just cultivate where the pawn currently is
            if (pawn.Faction == Faction.OfPlayer)
            {
                int lastCultSpotPriority = 0;
                IReadOnlyList<Pawn> readOnlyList = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
                foreach (Building building in pawn.Map.listerBuildings.allBuildingsColonist)
                {
                    if (!pawn.MapHeld.reservationManager.IsReserved(building))
                    {
                        if (building.HasComp<CompCultivationSpot>())
                        {
                            CompCultivationSpot cultivationSpot = building.GetComp<CompCultivationSpot>();
                            if (pawn.CanReach(building.Position, PathEndMode.OnCell, Danger.None))
                            {
                                if (lastCultSpotPriority < cultivationSpot.priority)
                                {
                                    if (cultivationSpot.elementType == "Any")
                                    {
                                        lastCultSpotPriority = cultivationSpot.priority;
                                        cultivationSpotThing = building;
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return cultivationSpotThing;
        }

        public static bool CanCultivateNow(Pawn pawn)
        {
            if (pawn.Faction.HostileTo(Faction.OfPlayer))//dont want hostiles cultivating
            {
                return false;
            }
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff == null)
            {
                return false;
            }
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            //if they are on qi gathering only and are at max qi they shouldnt cultivate:
            if (cultivatorHediff.autoCultivateType == 3)
            {
                if (qiPool != null)
                {
                    if (qiPool.amount >= qiPool.maxAmount)
                    {
                        return false;
                    }
                }
            }

            //if they are on essence realm cultivation only if they dont have enough qi to tribulate they shouldnt cultivate unless they can breakthrough;
            if (cultivatorHediff.autoCultivateType == 2)
            {
                Realm_Hediff essenceRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
                if (essenceRealm != null)
                {
                    if (qiPool != null)
                    {
                        //if they dont have enough qi to tribulate they shouldnt cultivate unless they can breakthrough;
                        int totalTribulationCost = (5 + (qiPool.maxAmount / 100)) * 10;
                        if (qiPool.amount < totalTribulationCost)
                        {
                            if (essenceRealm.progress < essenceRealm.maxProgress)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //if they dont have a qi pool they shouldnt cultivate on this mode unless they can breakthrogu
                        if (essenceRealm.progress < essenceRealm.maxProgress)
                        {
                            return false;
                        }
                    }
                }
            }

            if (cultivatorHediff.startTime != cultivatorHediff.endTime)//if its same then auto cultivation is basically disabled.
            {
                if (pawn.MapHeld != null)//dont want to cultivate on null maps
                {
                    float time = GenLocalDate.HourFloat(pawn.MapHeld);
                    if (time < cultivatorHediff.endTime && time > cultivatorHediff.startTime)//only can do jobs between assigned times
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        public static Job GetCultivationJob(Pawn pawn)
        {
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            Realm_Hediff essenceRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
            Realm_Hediff bodyRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
            if (cultivatorHediff != null && essenceRealm != null || bodyRealm != null && qiPool != null)
            {
                
                //this part does the cultivation jobs based on type
                switch (cultivatorHediff.autoCultivateType)
                {
                    case 1:
                        //auto realm
                        if (pawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm))
                        {
                            // Essence Realm cultivation, includes gathering qi for tribulation
                            int totalQiRefineCost = 2 + (qiPool.maxAmount / 10);
                            if (essenceRealm.progress >= essenceRealm.maxProgress)//auto attempt breakthrough when possible.
                            {
                                return JobMaker.MakeJob(AscensionDefOf.AS_BreakthroughEssence, pawn, FindCultivationSpot(pawn));
                            }
                            if (totalQiRefineCost > qiPool.maxAmount)
                            {
                                int totalInnerQiRefineCost = 2 + (qiPool.maxAmount / 50);//2 plus 2%
                                if (totalInnerQiRefineCost <= qiPool.amount)
                                {
                                    return JobMaker.MakeJob(AscensionDefOf.AS_RefineQiCauldronJob, pawn, FindCultivationSpot(pawn));
                                }else
                                {
                                    return JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, pawn, FindCultivationSpot(pawn));
                                }
                            }else if (totalQiRefineCost <= qiPool.amount)
                            {
                                return JobMaker.MakeJob(AscensionDefOf.AS_RefineQiJob, pawn, FindCultivationSpot(pawn));
                            }
                            else
                            {
                                return JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, pawn, FindCultivationSpot(pawn));
                            }
                        }else if (pawn.health.hediffSet.HasHediff(AscensionDefOf.BodyRealm))
                        {
                            // Body Realm cultivation. 
                            if (bodyRealm.progress >= bodyRealm.maxProgress)//auto attempt breakthrough when possible.
                            {
                                return JobMaker.MakeJob(AscensionDefOf.AS_BreakthroughBody, pawn, FindCultivationSpot(pawn));
                            }
                            return JobMaker.MakeJob(AscensionDefOf.AS_ExerciseJob, pawn, FindCultivationSpot(pawn));
                        }
                        return JobMaker.MakeJob(AscensionDefOf.AS_ExerciseJob, pawn, FindCultivationSpot(pawn));
                        break;
                    case 2:
                        //auto realm but without trib
                        if (pawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm))
                        {
                            // Essence Realm cultivation only, excludes gathering qi for tribulation
                            if (essenceRealm.progress >= essenceRealm.maxProgress)//auto attempt breakthrough when possible.
                            {
                                return JobMaker.MakeJob(AscensionDefOf.AS_BreakthroughEssence, pawn, FindCultivationSpot(pawn));
                            }
                        }
                        else if (pawn.health.hediffSet.HasHediff(AscensionDefOf.BodyRealm))
                        {
                            // Body Realm cultivation. 
                            if (bodyRealm.progress >= bodyRealm.maxProgress)//auto attempt breakthrough when possible.
                            {
                                return JobMaker.MakeJob(AscensionDefOf.AS_BreakthroughBody, pawn, FindCultivationSpot(pawn));
                            }
                            return JobMaker.MakeJob(AscensionDefOf.AS_ExerciseJob, pawn, FindCultivationSpot(pawn));
                        }
                        return JobMaker.MakeJob(AscensionDefOf.AS_ExerciseJob, pawn, FindCultivationSpot(pawn));
                        break;
                    case 3:
                        // Qi Gathering only. Just gather Qi instead of cultivating a realm.
                        return JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, pawn, FindCultivationSpot(pawn));
                        break;
                    default:
                        Log.Message("Ascension error autoCultivateType beyond normal allowed range.");
                        return JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, pawn, FindCultivationSpot(pawn));
                        break;
                }
            }else
            {
                return JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, pawn);
            }
        }
    }
}
