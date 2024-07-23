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
            SoulMendGameComponent smMapComp = Current.Game.GetComponent<SoulMendGameComponent>(); 
            if (smMapComp != null)
            {
                smMapComp.AddSoulMender(pawn);
            }
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            SoulMendGameComponent smMapComp = Current.Game.GetComponent<SoulMendGameComponent>();
            if (smMapComp != null)
            {
                smMapComp.RemoveSoulMender(pawn);
            }
        }

    }
}
