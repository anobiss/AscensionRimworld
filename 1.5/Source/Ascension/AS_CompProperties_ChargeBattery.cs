
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class AS_CompProperties_ChargeBattery : CompProperties_AbilityEffect
    {
        public int charge = 100;
        public AS_CompProperties_ChargeBattery()
        {
            compClass = typeof(AS_CompAbilityEffect_ChargeBattery);
        }
    }
}
