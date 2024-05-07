
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class DaoPool_Hediff : HediffWithComps
    {
        public override string SeverityLabel
        {
            get
            {
                string severityText = this.Severity.ToString("0.0");
                severityText += " Dao Energy";
                return severityText;
            }
        }
    }

}
