using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class HediffCompProperties_AddScrollReq : HediffCompProperties
    {
        public List<HediffDef> reqHediffsList = new List<HediffDef>();
        public HediffCompProperties_AddScrollReq()
        {
            compClass = typeof(HediffComp_AddScrollReq);
        }
    }
}
