using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class CompProperties_GatherQi : CompProperties
    {
        public int amount = 1;
        public int range = 1;

        public CompProperties_GatherQi()
        {
            compClass = typeof(CompGatherQi);
        }
    }
}

