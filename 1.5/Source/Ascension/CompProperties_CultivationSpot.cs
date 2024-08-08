
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

        public bool publicUse = true;

        public Pawn spotUser;//this is assigned to whoever is occupying it

        public int priority = 1;

        public ElementEmitMapComponent.Element elementType = ElementEmitMapComponent.Element.None;

        public int jobType = 0;//0 is Any, 1 is exercise, 2 qi gathering, 3 is qi refining, 4 is body breaktrough, 5 is essence breakthrough, 6 is gc breakthrough, 7 is inner cauldron refinement, 8 is foundation training

        public string realmType = "Any";
        public CompProperties_CultivationSpot()
        {
            compClass = typeof(CompCultivationSpot);
        }
    }
}

