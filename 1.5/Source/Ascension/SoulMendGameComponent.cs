using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class SoulMendGameComponent : GameComponent
    {

        private const float cost = 1f;
        private static AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        private bool logs = settings.logHealsBool;
        private int tickRate = (int)settings.SMTickRate;
        private int tickCount = 0;
        private List<Pawn> SoulMenders;
        public SoulMendGameComponent(Game game)
        {
            SoulMenders = new List<Pawn>();
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            tickCount--;
            if (tickCount <= 0)
            {
                logs = settings.logHealsBool;
                tickRate = (int)settings.SMTickRate;
                for (int i = SoulMenders.Count - 1; i >= 0; i--)
                {
                    Pawn soulMender = SoulMenders[i];
                    if (soulMender != null)
                    {
                        if (soulMender.MapHeld != null)
                        {
                            if (soulMender.Dead)
                            {
                                Hediff smHediff = soulMender.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.AS_SoulMend);
                                if (smHediff != null)//dont need to be greater than 1 because we remove 1 severity and hediff removes soul mender when removed.
                                {
                                    if (logs == true)
                                    {
                                        //Log.Message("attempted ressurect" + soulMender.Name);
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
                                        //Log.Message("removed no soul mend" + soulMender.Name);
                                    }

                                    SoulMenders.RemoveAt(i);
                                }
                            }
                        }else
                        {
                            //Log.Message("soul mender map null" + soulMender.Name);
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
                //Log.Message("added soul mender" + soulMender.Name);
            }
        }

        public void RemoveSoulMender(Pawn soulMender)
        {
            if (soulMender != null && SoulMenders.Contains(soulMender))
            {
                SoulMenders.Remove(soulMender);
                //Log.Message("removed soul mender" + soulMender.Name);
            }
        }
    }
}
