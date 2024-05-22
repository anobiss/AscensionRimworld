using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    //Qi Bur
    public class GameCondition_QiBurst : GameCondition
    {
        private int QiBurstDurationTicks = 2750;
        private int QiBurstDurationTicksDefault
        {
            get
            {
                if (base.Permanent)//in temp base its shorter
                {
                    return 999999;
                }
                return 2750;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref QiBurstDurationTicks, "curColorTransition", QiBurstDurationTicksDefault);
        }

        public override void Init()
        {
            base.Init();
            //foreach(Pawn pawn in this.map)
            //give all pawns with qipool qi resonance
        }

        public override void GameConditionTick()
        {
            if (!base.Permanent && base.TicksLeft > TransitionTicks)
            {
                base.TicksLeft = TransitionTicks;
            }
        }
    }

}
