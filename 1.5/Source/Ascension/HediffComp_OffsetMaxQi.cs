using Verse;
namespace Ascension
{
    public class HediffComp_OffsetMaxQi : HediffComp
    {
        public HediffCompProperties_OffsetMaxQi Props => (HediffCompProperties_OffsetMaxQi)props;
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            QiPool_Hediff qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                qiPool.maxAmountOffset += Props.offset;
                AscensionUtilities.UpdateQiMax(qiPool);
            }
        }
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            QiPool_Hediff qiPool = Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                qiPool.maxAmountOffset -= Props.offset;
                AscensionUtilities.UpdateQiMax(qiPool);
            }
        }
    }
}