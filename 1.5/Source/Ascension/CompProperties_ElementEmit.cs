using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class CompProperties_ElementEmit : CompProperties
    {
        public int amount = 1;
        public int range = 1;
        public string element = "None";

        public CompProperties_ElementEmit()
        {
            compClass = typeof(CompElementEmit);
        }
    }
}

