using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class SoulMendMapComponent : MapComponent
    {
        private const float cost = 1f;
        private bool logs = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().logHealsBool;
        private int tickRate = (int)LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().SMTickRate;
        private int tickCount;
        private List<Pawn> SoulMenders;
        public SoulMendMapComponent(Map map) : base(map)
        {
            SoulMenders = new List<Pawn>();
        }

        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();
            // Add any GUI code here if needed
        }

        public override void MapComponentTick()
        {

            base.MapComponentTick();

            if (tickCount <= 0)
            {
                logs = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().logHealsBool;
                tickRate = (int)LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().SMTickRate;
                for (int i = SoulMenders.Count - 1; i >= 0; i--)
                {
                    Pawn soulMender = SoulMenders[i];
                    if (soulMender != null)
                    {
                        if (soulMender.Dead)
                        {
                            Hediff smHediff = soulMender.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.AS_SoulMend);
                            if (smHediff != null)//dont need to be greater than 1 because we remove 1 severity and hediff removes soul mender when removed.
                            {
                                if (logs == true)
                                {
                                    Log.Message("attempted ressurect" + soulMender.Name);
                                }
                                ResurrectionUtility.TryResurrect(soulMender);
                                if (smHediff.Severity <= cost)//remember sm hediff removes itself from the list when its removed
                                {
                                    smHediff.Severity = 0;
                                }
                                else
                                {
                                    smHediff.Severity -= cost;
                                }
                            }
                            else
                            {
                                if (logs == true)
                                {
                                    Log.Message("removed no soul mend" + soulMender.Name);
                                }

                                SoulMenders.RemoveAt(i);
                            }
                        }
                    }
                    continue;
                }
                tickCount = tickRate;
            }
        }

        public void AddSoulMender(Pawn soulMender)
        {
            if (soulMender != null && !SoulMenders.Contains(soulMender))
            {
                SoulMenders.Add(soulMender);
            }
        }

        public void RemoveSoulMender(Pawn soulMender)
        {
            if (soulMender != null && SoulMenders.Contains(soulMender))
            {
                SoulMenders.Remove(soulMender);
            }
        }
    }
}
