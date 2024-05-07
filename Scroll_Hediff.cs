using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class Scroll_Hediff : HediffWithComps
    {
        //this class is for scrolls, it makes them invisible for use in dao tab
        public override bool Visible
        {
            get
            {
                return false;
            }
        }
    }
}
