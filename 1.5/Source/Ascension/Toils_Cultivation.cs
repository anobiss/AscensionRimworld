using System;
using Verse;
using Verse.AI;

namespace Ascension
{
    public static class Toils_Cultivation
    {
        private static readonly JobDef qiGatherDef = AscensionDefOf.AS_QiGatheringJob;
        private static readonly JobDef qiRefineDef = AscensionDefOf.AS_RefineQiJob;
        private static readonly JobDef exerciseDef = AscensionDefOf.AS_ExerciseJob;
        private static readonly JobDef icRefineDef = AscensionDefOf.AS_RefineQiCauldronJob;
        private static readonly JobDef bBreakDef = AscensionDefOf.AS_BreakthroughBody;
        private static readonly JobDef eBreakDef = AscensionDefOf.AS_BreakthroughEssence;
        public static Toil Wait(int ticks, JobDef cultivatioJob, TargetIndex face = TargetIndex.None)
        {
            Toil toil = ToilMaker.MakeToil("Wait");
            int tickCount = 0;
            toil.initAction = delegate
            {
                toil.actor.pather.StopDead();
                tickCount = 0;
                //sets ticks based on prog precent and given ticks then resets progress: this part loads/continues the job progress and resets it

            };
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            if (ticks > 1)
            {
                toil.defaultDuration = ticks;
            }
            else
            {
                toil.defaultDuration = 1;
            }
            if (face != 0)
            {
                toil.handlingFacing = true;
                toil.tickAction = delegate
                {
                    toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
                };
            }

            toil.tickAction = delegate
            {
                if (face != 0)
                {
                    toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
                }
                if (tickCount < ticks)
                {
                    tickCount++;
                    //Log.Message(toil.actor.Name + " tickCount ticks is " + tickCount.ToString("#"));
                    Cultivator_Hediff cultivatorHediff = toil.actor.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                    if (cultivatorHediff != null)
                    {
                        if (cultivatioJob == AscensionDefOf.AS_QiGatheringJob)
                        {
                            cultivatorHediff.qiGatheringJobProg += 1; // we add to it incase they dont finish multiple jobs
                            //Log.Message(toil.actor.Name + " prog is " + cultivatorHediff.qiGatheringJobProg.ToString("#"));
                        }
                        if (cultivatioJob == AscensionDefOf.AS_RefineQiJob)
                        {
                            cultivatorHediff.refineQiJobProg += 1;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_ExerciseJob)
                        {
                            cultivatorHediff.exerciseJobProg += 1;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_RefineQiCauldronJob)
                        {
                            cultivatorHediff.refineICJobProg += 1;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_BreakthroughBody)
                        {
                            cultivatorHediff.bodyBreakthrouchJobProg += 1;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_BreakthroughEssence)
                        {
                            cultivatorHediff.essenceBreakthrouchJobProg += 1;
                        }
                    }else
                    {
                        //Log.Message(toil.actor.Name + " cultivator hediff null");
                    }
                }

            };
            return toil;
        }
        public static Toil CalculateDuration(int baseDurationTicks, Toil waitToil, JobDef cultivationJob)
        {
            Toil calculateDurationToil = new Toil();
            calculateDurationToil.initAction = delegate
            {
                int calculatedDurationTicks = baseDurationTicks;
                Cultivator_Hediff cultivatorHediff = calculateDurationToil.actor.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                if (cultivatorHediff != null)
                {
                    float cultivationTicks = baseDurationTicks / AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                    calculatedDurationTicks = (int)Math.Floor(cultivationTicks);

                    if (cultivationJob == qiGatherDef && cultivatorHediff.qiGatheringJobProg > 0)
                    {
                        //Log.Message(calculateDurationToil.actor.Name + " continueing job progress " + cultivatorHediff.qiGatheringJobProg);
                        //Log.Message(calculateDurationToil.actor.Name + "precalc ticks: " + calculatedDurationTicks);
                        calculatedDurationTicks -= cultivatorHediff.qiGatheringJobProg;
                        //Log.Message(calculateDurationToil.actor.Name + "postcalc ticks: " + calculatedDurationTicks);
                        //Log.Message(calculateDurationToil.actor.Name + " qiGatheringJobProg job progress " + cultivatorHediff.qiGatheringJobProg);

                    }
                    if (cultivationJob == qiRefineDef && cultivatorHediff.refineQiJobProg > 0)
                    {
                        calculatedDurationTicks -= cultivatorHediff.refineQiJobProg;
                    }
                    if (cultivationJob == exerciseDef && cultivatorHediff.exerciseJobProg > 0)
                    {
                        calculatedDurationTicks -= cultivatorHediff.exerciseJobProg;
                    }
                    if (cultivationJob == icRefineDef && cultivatorHediff.refineICJobProg > 0)
                    {
                        calculatedDurationTicks -= cultivatorHediff.refineICJobProg;
                    }
                    if (cultivationJob == bBreakDef && cultivatorHediff.bodyBreakthrouchJobProg > 0)
                    {
                        calculatedDurationTicks -= cultivatorHediff.bodyBreakthrouchJobProg;
                    }
                    if (cultivationJob == eBreakDef && cultivatorHediff.essenceBreakthrouchJobProg > 0)
                    {
                        calculatedDurationTicks -= cultivatorHediff.essenceBreakthrouchJobProg;
                    }
                }

                waitToil.defaultDuration = calculatedDurationTicks;
            };
            return calculateDurationToil;
        }
    }
}
