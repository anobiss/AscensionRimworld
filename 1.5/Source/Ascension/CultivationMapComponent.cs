using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Ascension
{
    //this map component stores the cultivation spot list for easy access and so we dont have to search constantly.
    public class CultivationMapComponent : MapComponent
    {
        public List<CompCultivationSpot> CultivationSpots; // list of cultivation spots on the map, not saved or loaded because just remake it when spawn ordespawn
        public CultivationMapComponent(Map map)
            : base(map)
        {
            CultivationSpots = new List<CompCultivationSpot>();
        }
    }
}