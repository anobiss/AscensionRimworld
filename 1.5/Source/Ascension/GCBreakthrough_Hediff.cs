using System;
using Verse;

namespace Ascension
{
    public class GCBreakthrough_Hediff : HediffWithComps
    {
        public override void PostTick()
        {
            base.PostTick();
            //optimize later
            if (pawn.CurJobDef != AscensionDefOf.AS_GoldenCoreBreakthrough)
            {
                Severity = 0;
            }
        }
    }
}
