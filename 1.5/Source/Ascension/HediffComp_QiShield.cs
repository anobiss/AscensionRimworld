
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Ascension
{
    //uses 100 qi to prevent one instance of damage
    public class HediffComp_QiShield : HediffComp
    {
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            //later need to only do fake lightning strike if it is visible as it is purely visual effect.
            //base.CompPostMake();
            if (Pawn != null && Find.CurrentMap != null)
            {
                if (Pawn.Position != null)
                {
                    Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_FakeLightningStrike(Find.CurrentMap, parent.pawn.Position));
                }
            }
        }
    }
}