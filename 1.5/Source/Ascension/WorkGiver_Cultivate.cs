using RimWorld;
using Verse;
using Verse.AI;

namespace Ascension
{
    public class WorkGiver_Cultivate : WorkGiver
    {
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return !CultivationJobUtility.CanCultivateNow(pawn);
        }

        public override Job NonScanJob(Pawn pawn)
        {
            if (CultivationJobUtility.CanCultivateNow(pawn))
            {
                return CultivationJobUtility.GetCultivationJob(pawn);
            }
            return null;
        }
    }
}
