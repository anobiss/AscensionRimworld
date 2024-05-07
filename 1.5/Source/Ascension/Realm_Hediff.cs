using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    //hediffs for body and essence realms.
    public class Realm_Hediff : HediffWithComps
    {
        public int progress = 0;
        public int maxProgress = 100;


        //would be more efficeint to use ints but would lead to confusion and is very negligable due to how often we check this.
        public override string SeverityLabel
        {
            get
            {
                string severityText;
                severityText = progress + "/"+maxProgress;
                return severityText;
            }
        }

        //sets label to stage label
        public override string Label
        {
            get
            {
                string stageLabel;
                stageLabel = this.CurStage.label;
                return stageLabel;
            }
        }

        public override void ExposeData()
        {
            //a little ineffecient since on load this is called around three times.
            if (this.Severity > 1)
            {
                AscensionUtilities.UpdateMaxProg(this);
            }
            else
            {
                maxProgress = 100;
            }
            Scribe_Values.Look(ref progress, "progress");
            Scribe_Values.Look(ref progress, "maxProgress");
            base.ExposeData();
        }
    }
}
