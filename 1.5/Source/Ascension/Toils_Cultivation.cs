using System;
using Verse;
using Verse.AI;

namespace Ascension
{
    public static class Toils_Cultivation
    {
        public static Toil CalculateDuration(int baseDurationTicks, Toil waitToil)
        {
            Toil calculateDurationToil = new Toil();
            calculateDurationToil.initAction = delegate
            {
                int calculatedDurationTicks = baseDurationTicks;
                Cultivator_Hediff cultivatorHediff = calculateDurationToil.actor.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
                if (cultivatorHediff != null)
                {
                    float cultivationTicks = baseDurationTicks / AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff);
                    calculatedDurationTicks = (int)Math.Floor(cultivationTicks);
                }
                waitToil.defaultDuration = calculatedDurationTicks;
            };
            return calculateDurationToil;
        }
    }
}
