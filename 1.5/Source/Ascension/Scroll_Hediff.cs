using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class Scroll_Hediff : HediffWithComps
    {
        public static float mastery = 0;
        //this class is for scrolls, it makes them invisible for use in dao tab
        public override bool Visible
        {
            get
            {
                return false;
            }
        }
        public override void ExposeData()
        {
            Scribe_Values.Look(ref mastery, "mastery");
            base.ExposeData();
        }
    }
}
