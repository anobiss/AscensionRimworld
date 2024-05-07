
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class SpiritSword_Hediff : HediffWithComps
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            if (pawn.abilities != null)
            {
                pawn.abilities.GainAbility(AscensionDefOf.ManifestSpiritSword);
            }
        }
    }
}
