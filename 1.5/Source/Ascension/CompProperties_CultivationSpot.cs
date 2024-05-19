
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

        public Pawn spotUser;//this is assigned to whoever is occupying it

        public int priority = 1;

        public string elementType = "Any";

        public string realmType = "Any";
        public CompProperties_CultivationSpot()
        {
            compClass = typeof(CompCultivationSpot);
        }
    }
}

