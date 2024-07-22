using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;

namespace Ascension
{
    public class AS_CompAbilityEffect_StrikeLightning : CompAbilityEffect
    {
        public new AS_CompProperties_AbilityStrikeLightning Props => (AS_CompProperties_AbilityStrikeLightning)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            StrikeLightning(target);
        }

        private void StrikeLightning(LocalTargetInfo target)
        {
            Pawn pawn = parent.pawn;
            pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(pawn.Map, target.Cell));
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
