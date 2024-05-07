using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Ascension
{
    //converts dao energy into progression on foundation or p immortality if max foundation(12), if none then lightning strike the pawn and remove/end tribulation.
    public class HediffComp_TribulationConversion : HediffComp
    {
        private int ticksToConversion = 1000;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToConversion--;
            if (ticksToConversion <= 0)
            {
                ticksToConversion = 1000;
                if (this.Pawn.health.hediffSet.HasHediff(AscensionDefOf.DaoPool, false))
                {
                    if (this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.DaoPool, false).Severity >= 1)
                    {
                        if (this.Pawn.health.hediffSet.HasHediff(AscensionDefOf.AscendantFoundation, false))
                        {
                            if (this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.AscendantFoundation, false).Severity >= 12)
                            {
                                if (this.Pawn.health.hediffSet.HasHediff(AscensionDefOf.PseudoImmortality, false))
                                {
                                    if (this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.PseudoImmortality, false).Severity >= 77)
                                    {
                                        Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, parent.pawn.Position));
                                        this.parent.Severity = 0f;
                                        Log.Message($"Ended Tribulation. Max Psuedo-Imortality");
                                    }
                                    else
                                    {
                                        this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.PseudoImmortality, false).Severity += 0.1f;
                                        Log.Message($"Added severity to PseudoImmortality. Next conversion will take {ticksToConversion} ticks.");
                                    }
                                }
                                else
                                {
                                    this.Pawn.health.AddHediff(AscensionDefOf.PseudoImmortality);
                                    this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.PseudoImmortality, false).Severity = 0.1f;
                                    Log.Message($"Added PseudoImmortality. Next conversion will take {ticksToConversion} ticks.");
                                }
                            }
                            else
                            {
                                this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.AscendantFoundation, false).Severity += 0.1f;
                                Log.Message($"Added severity to AscendantFoundation. Next conversion will take {ticksToConversion} ticks.");
                            }
                        }
                        else
                        {
                            this.Pawn.health.AddHediff(AscensionDefOf.AscendantFoundation);
                            this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.AscendantFoundation, false).Severity = 0.1f;
                            Log.Message($"Added AscendantFoundation. Next conversion will take {ticksToConversion} ticks.");
                        }
                        Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_FakeLightningStrike(Find.CurrentMap, parent.pawn.Position));
                        this.Pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.DaoPool, false).Severity -= 1f;
                        this.parent.Severity -= 0.1f;
                    } else
                    {
                        Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, parent.pawn.Position));
                        this.parent.Severity = 0f;
                        Log.Message($"Ended Tribulation. Not enough dao energy.");
                    }
                }
                else
                {
                    Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, parent.pawn.Position));
                    this.parent.Severity = 0f;
                    Log.Message($"Ended Tribulation. No dao pool.");
                }
            }
        }
        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToConversion, "ticksToConversion", 0, false);
        }
        public override string CompDebugString()
        {
            return "ticksToConversion: " + this.ticksToConversion;
        }
    }
}