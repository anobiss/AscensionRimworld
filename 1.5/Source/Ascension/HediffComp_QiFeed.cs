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
            Pawn_NeedsTracker needs = this.Pawn.needs;
            QiPool_Hediff qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                if (qiPool.amount >= 10000f)
                {
                    if (needs.food != null)
                    {
                        if (needs.food.CurCategory == HungerCategory.Hungry)
                        {
                            needs.food.CurLevel = 1f;
                            qiPool.amount -= 10000f;
                        }
                    }
                }
            }
        }

    }
}