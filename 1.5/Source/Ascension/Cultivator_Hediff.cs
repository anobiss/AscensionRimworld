
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using static RimWorld.PsychicRitualRoleDef;
using static Verse.SpecificApparelRequirement;

namespace Ascension
{
    public class Cultivator_Hediff : HediffWithComps
    {
        public float startTime = 10f;
        public float endTime = 12f;
        public int autoCultivateType = 1;
        public float cultivationSpeed = 1;//this is what cultivation jobs read.
        public float cultivationBaseSpeed = 1;//this isnt readonly because we want to allow boosting this later.
        public float cultivationSpeedOffset = 1;//base is multiplied with this 

        public int qiRecoveryAmount = 0;
        public float qiRecoverySpeed = 1;
        public float qiRecoverySpeedOffset = 0;


        //inner cauldron/anima conversion. cap is ignored when anima conversion.
        public int innerCauldronLimit = 1200;
        public int innerCauldronQi = 0;


        public override bool Visible
        {
            get
            {
                return false;
            }
        }
        public override void ExposeData()
        {
            Scribe_Values.Look(ref qiRecoverySpeed, "qiRecoverySpeed");
            Scribe_Values.Look(ref qiRecoverySpeedOffset, "qiRecoverySpeedOffset");

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
