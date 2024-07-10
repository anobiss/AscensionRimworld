using Verse;

namespace Ascension
{
    public class SoulMend_Hediff : HediffWithComps
    {
        public override string SeverityLabel
        {
            get
            {
                return "AS_SoulMendSeverity".Translate(Severity.ToString("#").Named("SM"));
            }
        }
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            SoulMendMapComponent smMapComp = pawn.Map.GetComponent<SoulMendMapComponent>();
            if (smMapComp != null)
            {
                smMapComp.AddSoulMender(pawn);
            }
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            SoulMendMapComponent smMapComp = pawn.Map.GetComponent<SoulMendMapComponent>();
            if (smMapComp != null)
            {
                smMapComp.RemoveSoulMender(pawn);
            }
        }

    }
}
