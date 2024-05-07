
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class Tribulation_Hediff : HediffWithComps
    {
        public override string SeverityLabel
        {
            get
            {
                string severityLabel = ((int)(this.Severity * 10))+"AS_TConversionsLeft".Translate();
                return severityLabel;
            }
        }
    }

}
