
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using static RimWorld.PsychicRitualRoleDef;
using static Verse.SpecificApparelRequirement;
using Random = System.Random;

namespace Ascension
{
    public class Cultivator_Hediff : HediffWithComps
    {
        public float breakthroughChance = 0f;
        public float breakthroughChanceOffset = 0f;

        public float startTime = 10f;
        public float endTime = 12f;
        public int autoCultivateType = 1;
        public float cultivationSpeed = 1f;//this is what cultivation jobs read.
        public float cultivationBaseSpeed = 1f;//this isnt readonly because we want to allow boosting this later.
        public float cultivationSpeedOffset = 1f;//base is multiplied with this 

        public float qiRecoveryAmount = 0f;
        public float qiRecoverySpeed = 1f;
        public float qiRecoverySpeedOffset = 0f; // qiRecovery is recalced not saved


        //golden core score and if they have one. if they dont its 0. score is set when golden core breakthrough is finished.
        public float goldenCoreScore = 0;
        

        //inner cauldron/anima conversion. cap is ignored when anima conversion.
        public float innerCauldronLimit = 1200;
        public float innerCauldronQi = 0;


        public ElementEmitMapComponent.Element element = ElementEmitMapComponent.Element.None;

        public override void PostMake()
        {
            base.PostMake();
            if (element == ElementEmitMapComponent.Element.None)
            {
                // Array of possible elements
                ElementEmitMapComponent.Element[] possibleElements = new ElementEmitMapComponent.Element[]
                {
            ElementEmitMapComponent.Element.Water,
            ElementEmitMapComponent.Element.Fire,
            ElementEmitMapComponent.Element.Earth,
            ElementEmitMapComponent.Element.Metal,
            ElementEmitMapComponent.Element.Wood
                };

                // Create a new random number generator
                Random random = new Random();

                // Assign a random element from the array
                element = possibleElements[random.Next(possibleElements.Length)];
            }
        }
        public override bool Visible
        {
            get
            {
                return false;
            }
        }
        public override void ExposeData()
        {
            Scribe_Values.Look(ref goldenCoreScore, "goldenCoreScore");
            Scribe_Values.Look(ref qiRecoverySpeed, "qiRecoverySpeed");
            //Scribe_Values.Look(ref qiRecoverySpeedOffset, "qiRecoverySpeedOffset");
            Scribe_Values.Look(ref element, "element");
            Scribe_Values.Look(ref cultivationSpeedOffset, "cultivationSpeedOffset");
            Scribe_Values.Look(ref cultivationBaseSpeed, "cultivationBaseSpeed");
            Scribe_Values.Look(ref cultivationSpeed, "cultivationSpeed");
            Scribe_Values.Look(ref startTime, "startTime");
            Scribe_Values.Look(ref endTime, "endTime");
            Scribe_Values.Look(ref autoCultivateType, "autoCultivateType");
            Scribe_Values.Look(ref innerCauldronLimit, "innerCauldronLimit");
            Scribe_Values.Look(ref innerCauldronQi, "innerCauldronQi");
            base.ExposeData();
        }
    }
}
