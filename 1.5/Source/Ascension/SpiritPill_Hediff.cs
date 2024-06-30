using Verse;

namespace Ascension
{
    //hediffs for body and essence realms.
    public class SpiritPill_Hediff : HediffWithComps
    {
        public override string SeverityLabel
        {
            get
            {
                string severityText;
                severityText = "AS_SpiritPillSeverityLabel".Translate(Severity.ToString("#").Named("TIER"));
                return severityText;
            }
        }
        public override string Label
        {
            get
            {
                string stageLabel;
                stageLabel = this.CurStage.label;
                return stageLabel;
            }
        }
    }
}
