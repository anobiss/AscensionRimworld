
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions.Must;
using Verse;
using Verse.Noise;

namespace Ascension
{
    public class AS_CompAbilityEffect_ShootThing : CompAbilityEffect
    {
        public new AS_CompProperties_ShootThing Props => (AS_CompProperties_ShootThing)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            ShootThing(target);
        }

        private void ShootThing(LocalTargetInfo target)
        {
            if (target != null && target.Thing.Map != null)
            {
                Thing ShotThing = ThingMaker.MakeThing(Props.shotThing);
                GenPlace.TryPlaceThing(ShotThing, target.Cell, target.Thing.Map, ThingPlaceMode.Near, null, null, default);
            }
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Pawn != null;
        }
    }
}
