using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;
using System.Security.Cryptography;

namespace Ascension
{

    public class JobGiver_Cultivate : ThinkNode_JobGiver
    {
        public override float GetPriority(Pawn pawn)
        {
            if (CultivationJobUtility.CanCultivateNow(pawn))
            {
                float priority = 7.2f;
                if (pawn.workSettings != null)
                {
                    priority = pawn.workSettings.GetPriority(AscensionDefOf.Cultivation);
                }
                return priority;
            }
            return 0f;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!CultivationJobUtility.CanCultivateNow(pawn))//checks if its the right time to cultivate 
            {
                return null;
            }
            return CultivationJobUtility.GetCultivationJob(pawn);//runs always, incase they have a spot to cultivate. leads to cultivation job
            //this should include a walk to cultivation spot job
        }
    }
}