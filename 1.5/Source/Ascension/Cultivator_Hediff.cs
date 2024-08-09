
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;
using static RimWorld.PsychicRitualRoleDef;
using static Verse.SpecificApparelRequirement;
using Random = System.Random;

namespace Ascension
{
    public class Cultivator_Hediff : HediffWithComps
    {
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        public readonly int fMax = 1200;
        public enum LawType
        {
            None = 0,
            Body = 1,
            Essence = 2,
        }
        public ElementEmitMapComponent.Element element = ElementEmitMapComponent.Element.None;

        //chosen law is overwritten for random cultivators
        public LawType chosenLawType = LawType.Essence; //actual default is loaded during scribe. this is just a placeholder

        public LawType lawType = LawType.None; //stores characters permanent law type, should only be changed when assigned.

        //store if the law is confirmed for auto-choosing
        public bool confirmedLaw = false;
        //store if the 
        public int ticksToDeAge = 0;

        public int Foundation = 0;

        public float breakthroughChance = 0f;
        public float breakthroughChanceOffset = 0f;

        public float startTime = 10f;
        public float endTime = 12f;
        public int autoCultivateType = 1;
        public float cultivationSpeed = 1f;//this is what cultivation jobs read.
        public float cultivationBaseSpeed = 1f;//this isnt readonly because we want to allow boosting this later.
        public float cultivationSpeedOffset = 1f;//base is multiplied with this 

        //golden core score and if they have one. if they dont its 0. score is set when golden core breakthrough is finished.
        public float goldenCoreScore = 0;
        

        //inner cauldron/anima conversion. cap is ignored when anima conversion.
        public float innerCauldronLimit = 1200;
        public float innerCauldronQi = 0;


        //we only minus the ticks by these to only really give back the progress they put in
        public int foundationTrainingJobProg = 0;
        public int qiGatheringJobProg = 0; //0.99 is 99% done
        public int refineQiJobProg = 0;
        public int refineICJobProg = 0;
        public int exerciseJobProg = 0;
        public int bodyBreakthrouchJobProg = 0;
        public int essenceBreakthrouchJobProg = 0;

        public float foundationTrainingOffset = 1f;

        public override void PostMake()
        {
            base.PostMake();
            if (element == ElementEmitMapComponent.Element.None)
            {
                element = AscensionUtilities.AssignElement();
            }
        }

        public float lifespan = -1000f;//to show we havent set it which is a problem
        public float lifespanEfficiency = 1f;
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            AscensionUtilities.GetBaseLifespan(this);
            AscensionUtilities.UpdateLifespanEfficiency(this);
            if (element == ElementEmitMapComponent.Element.None)
            {
                element = AscensionUtilities.AssignElement();
            }
            if (lifespan == 0)
            {
                lifespan = AscensionUtilities.GetBaseLifespan(this);
            }
        }
        public float lastCheckAge = 0f;
        public override void PostTick()//lifespan logic here
        {
            base.PostTick();
            this.ticksToDeAge--;
            if (this.ticksToDeAge <= 0)
            {
                if (pawn.RaceProps != null)
                {
                    if (pawn.ageTracker != null)
                    {
                        if (lifespan == -1000f)
                        {
                            lifespan = AscensionUtilities.GetBaseLifespan(this);
                        }
                        if (lastCheckAge != 0f)
                        {
                            float yearsPast = pawn.ageTracker.AgeBiologicalYearsFloat - lastCheckAge;//3,600,000	ticks per year
                            lifespan -= yearsPast;// should add if de aged since last check too
                            //Log.Message(pawn.Name.ToString()+"decreased lifespan by yearspast"+yearsPast);
                        }
                        lastCheckAge = pawn.ageTracker.AgeBiologicalYearsFloat;

                        if (lifespan > pawn.RaceProps.lifeExpectancy)
                        {
                            float newAgeYears = AscensionUtilities.LifespanDeAge(this);
                            if (pawn.ageTracker.AgeBiologicalYearsFloat > newAgeYears && newAgeYears != 0)
                            {
                                if (newAgeYears != 0)
                                {
                                    float yearsReduced = pawn.ageTracker.AgeBiologicalYearsFloat - newAgeYears;
                                    float lifespanCost = yearsReduced / AscensionUtilities.UpdateLifespanEfficiency(this);
                                    if (lifespan >= lifespanCost + pawn.RaceProps.lifeExpectancy)
                                    {
                                        lifespan -= lifespanCost;
                                        //Log.Message($"Years Reduced: {yearsReduced}, Lifespan Efficiency: {lifespanEfficiency}, Calculated Cost: {lifespanCost}");
                                        pawn.ageTracker.AgeBiologicalTicks = (int)(newAgeYears * 3600000);//3,600,000	ticks per year
                                        pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);

                                    }
                                }
                            }
                        }
                    }
                }
                ticksToDeAge = (int)settings.LifespanTickRate;
            }

        }
        public override bool Visible
        {
            get
            {
                return false;
            }
        }
        public LawType DefaultChosen()
        {
            if (settings.defaultEssenceType)
            {
                return LawType.Essence;
            }else
            {
                return LawType.Body;
            }
        }
        public override void ExposeData()
        {
            

            Scribe_Values.Look(ref lifespan, "lifespan", -1000f);
            Scribe_Values.Look(ref lifespanEfficiency, "lifespanEfficiency", 1f);
            Scribe_Values.Look(ref ticksToDeAge, "ticksToDeAge", 0);
            Scribe_Values.Look(ref lastCheckAge, "lastCheckAge", 0f);//float for big numbers

            Scribe_Values.Look(ref confirmedLaw, "confirmedLaw");
            Scribe_Values.Look(ref Foundation, "Foundation");
            Scribe_Values.Look(ref foundationTrainingJobProg, "foundationTrainingJobProg");
            Scribe_Values.Look(ref qiGatheringJobProg, "qiGatheringJobProg");
            Scribe_Values.Look(ref exerciseJobProg, "exerciseJobProg");
            Scribe_Values.Look(ref bodyBreakthrouchJobProg, "bodyBreakthrouchJobProg");
            Scribe_Values.Look(ref essenceBreakthrouchJobProg, "essenceBreakthrouchJobProg");
            Scribe_Values.Look(ref refineQiJobProg, "refineQiJobProg");
            Scribe_Values.Look(ref refineICJobProg, "refineICJobProg");
            Scribe_Values.Look(ref goldenCoreScore, "goldenCoreScore");// this, the cultivators element and inner cauldron should be the only things we NEED to store permanently. 
            Scribe_Values.Look(ref element, "element");
            Scribe_Values.Look(ref chosenLawType, "chosenLawType", DefaultChosen());
            Scribe_Values.Look(ref lawType, "lawType");
            Scribe_Values.Look(ref startTime, "startTime");
            Scribe_Values.Look(ref endTime, "endTime");
            Scribe_Values.Look(ref autoCultivateType, "autoCultivateType");
            Scribe_Values.Look(ref innerCauldronLimit, "innerCauldronLimit");
            Scribe_Values.Look(ref innerCauldronQi, "innerCauldronQi");
            base.ExposeData();
        }
    }
}
