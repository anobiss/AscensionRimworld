using Verse;
using RimWorld;

namespace Ascension
{
    // Qi Burst Game Condition
    public class GameCondition_QiBurst : GameCondition
    {
        private const int SpiritTreeConversionInterval = 17500;
        private const float SpiritTreeConversionChance = 0.5f;
        private int ticksTilSpiritTreeConversion = SpiritTreeConversionInterval;
        public override int TransitionTicks => 180000;

        public override void Init()
        {
            base.Init();
            foreach (Map map in this.AffectedMaps)
            {
                QiGatherMapComponent qiMapComp = map.GetComponent<QiGatherMapComponent>();
                qiMapComp.AddQiGatherAt(100, 100, 500, 120);
            }
            // Initialization logic for pawns with Qi resonance
            foreach (Pawn pawn in this.SingleMap.mapPawns.AllPawns)
            {
                if (pawn.health != null && pawn.health.hediffSet.HasHediff(AscensionDefOf.Cultivator))
                {
                    // Add Qi resonance logic here
                    pawn.health.AddHediff(AscensionDefOf.QiResonance);
                }
            }
        }

        public override void End()
        {
            foreach (Map map in this.AffectedMaps) // before base in case end removes the map
            {
                QiGatherMapComponent qiMapComp = map.GetComponent<QiGatherMapComponent>();
                qiMapComp.RemoveQiGatherAt(100, 100, 500, 120);
            }
            base.End();
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();

            // Handle spirit tree conversion attempts
            ticksTilSpiritTreeConversion--;
            if (ticksTilSpiritTreeConversion <= 0)
            {
                ticksTilSpiritTreeConversion = SpiritTreeConversionInterval;

                foreach (Map map in this.AffectedMaps)
                {
                    Log.Message("Attempting spirit tree conversion");
                    if (Rand.Range(0f, 1f) < SpiritTreeConversionChance)
                    {
                        IncidentParms incidentParms = new IncidentParms();
                        incidentParms.target = map;
                        if (AscensionDefOf.SpiritTreeConversion.Worker.TryExecute(incidentParms))
                        {
                            Log.Message("Spirit tree conversion succeeded");
                        }
                        else
                        {
                            Log.Message("Spirit tree conversion failed");
                        }
                    }
                    else
                    {
                        Log.Message("Spirit tree conversion roll failed");
                    }
                }
            }

            // Ensure the condition duration is respected for non-permanent conditions
            if (!base.Permanent && base.TicksLeft > TransitionTicks)
            {
                base.TicksLeft = TransitionTicks;
            }
        }
    }
}
