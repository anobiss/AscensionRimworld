using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Ascension
{

    //dao hediff is gained from consuming Dao Energy

    //Dao Pool: This being has a pool of Dao Energy within its soul. When this being suffers from malnutrition thier body will convert Dao Energy within this pool into nutrients, feeding it.


    //this comp checks if they have malnutrition then feeds them and reduces the parent hediffs severity.
    public class HediffComp_QiFeed : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            QiPool_Hediff qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                if (qiPool.amount > 10000)
                {
                    if (this.Pawn.needs.food.Starving)
                    {
                        this.Pawn.needs.food.CurLevel = this.Pawn.RaceProps.FoodLevelPercentageWantEat;
                        qiPool.amount -= 10000;
                    }
                }
            }
        }

    }
}