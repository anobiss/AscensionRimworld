﻿
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class CompProperties_CultivationSpot : CompProperties
    {
        public bool occupied = false;

        public int priority = 1;
        public CompProperties_CultivationSpot()
        {
            compClass = typeof(CompCultivationSpot);
        }
    }
}

