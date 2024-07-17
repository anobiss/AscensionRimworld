using System;
using Verse;
using Verse.AI;

namespace Ascension
{
    public static class Toils_Cultivation
    {
        public static Toil Wait(int ticks, JobDef cultivatioJob, TargetIndex face = TargetIndex.None)
        {
            Toil toil = ToilMaker.MakeToil("Wait");
            JobDef qiGatherDef = AscensionDefOf.AS_QiGatheringJob;
            JobDef qiRefineDef = AscensionDefOf.AS_RefineQiJob;
            JobDef exerciseDef = AscensionDefOf.AS_ExerciseJob;
            JobDef icRefineDef = AscensionDefOf.AS_RefineQiCauldronJob;
            JobDef bBreakDef = AscensionDefOf.AS_BreakthroughBody;
            JobDef eBreakDef = AscensionDefOf.AS_BreakthroughEssence;

            int tickCount = 0;
            toil.initAction = delegate
            {
                toil.actor.pather.StopDead();
                tickCount = 0;
                //sets ticks based on prog precent and given ticks then resets progress: this part loads/continues the job progress and resets it
                Cultivator_Hediff cultivatorHediff = toil.actor.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                if (cultivatorHediff != null)
                {
                    if (cultivatioJob == qiGatherDef && cultivatorHediff.qiGatheringJobProg > 0)
                    {
                        Log.Message(toil.actor.Name + " continueing job progress " + cultivatorHediff.qiGatheringJobProg);
                        Log.Message(toil.actor.Name + "precalc ticks: " + ticks);
                        ticks -= cultivatorHediff.qiGatheringJobProg;
                        Log.Message(toil.actor.Name + "postcalc ticks: " + ticks);
                        Log.Message(toil.actor.Name + " qiGatheringJobProg job progress " + cultivatorHediff.qiGatheringJobProg);

                    }
                    if (cultivatioJob == qiRefineDef && cultivatorHediff.refineQiJobProg > 0)
                    {
                        Log.Message(toil.actor.Name + " continueing job progress " + cultivatorHediff.refineQiJobProg);
                        ticks -= cultivatorHediff.refineQiJobProg;
                    }
                    if (cultivatioJob == exerciseDef && cultivatorHediff.exerciseJobProg > 0)
                    {
                        Log.Message(toil.actor.Name + " continueing job progress " + cultivatorHediff.exerciseJobProg);
                        ticks -=  cultivatorHediff.exerciseJobProg;
                    }
                    if (cultivatioJob == icRefineDef && cultivatorHediff.refineICJobProg > 0)
                    {
                        Log.Message(toil.actor.Name + " continueing job progress " + cultivatorHediff.refineICJobProg);
                        ticks -=  cultivatorHediff.refineICJobProg;
                    }
                    if (cultivatioJob == bBreakDef && cultivatorHediff.bodyBreakthrouchJobProg > 0)
                    {
                        Log.Message(toil.actor.Name + " continueing job progress " + cultivatorHediff.bodyBreakthrouchJobProg);
                        ticks -= cultivatorHediff.bodyBreakthrouchJobProg;
                    }
                    if (cultivatioJob == eBreakDef && cultivatorHediff.essenceBreakthrouchJobProg > 0)
                    {
                        Log.Message(toil.actor.Name + " continueing job progress " + cultivatorHediff.essenceBreakthrouchJobProg);
                        ticks -= cultivatorHediff.essenceBreakthrouchJobProg;
                    }
                }
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
                    Log.Message(toil.actor.Name + " tickCount ticks is " + tickCount.ToString("#"));
                    Cultivator_Hediff cultivatorHediff = toil.actor.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                    if (cultivatorHediff != null)
                    {
                        if (cultivatioJob == AscensionDefOf.AS_QiGatheringJob)
                        {
                            cultivatorHediff.qiGatheringJobProg += tickCount; // we add to it incase they dont finish multiple jobs
                            Log.Message(toil.actor.Name + " prog is " + cultivatorHediff.qiGatheringJobProg.ToString("#"));
                        }
                        if (cultivatioJob == AscensionDefOf.AS_RefineQiJob)
                        {
                            cultivatorHediff.refineQiJobProg += tickCount;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_ExerciseJob)
                        {
                            cultivatorHediff.exerciseJobProg += tickCount;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_RefineQiCauldronJob)
                        {
                            cultivatorHediff.refineICJobProg += tickCount;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_BreakthroughBody)
                        {
                            cultivatorHediff.bodyBreakthrouchJobProg += tickCount;
                        }
                        if (cultivatioJob == AscensionDefOf.AS_BreakthroughEssence)
                        {
                            cultivatorHediff.essenceBreakthrouchJobProg += tickCount;
                        }
                    }else
                    {
                        Log.Message(toil.actor.Name + " cultivator hediff null");
                    }
                }
            };
            return toil;
        }

        public static Toil CalculateDuration(int baseDurationTicks, Toil waitToil)
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
                }
                waitToil.defaultDuration = calculatedDurationTicks;
            };
            return calculateDurationToil;
        }
    }
}
