using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using static HarmonyLib.Code;

namespace Ascension
{
    public class IngestionOutcomeDoer_SpiritPill : IngestionOutcomeDoer
    {
        private float GetQualitySeverity(Thing thing)
        {
            QualityCategory qc = new QualityCategory();
            QualityUtility.TryGetQuality(thing, out qc);

            if ((int)qc > 0)
            {
                return (float)qc;
            }
            return 1f;
        }
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {

            float tier = GetQualitySeverity(ingested);
            float spiritCost = AscensionUtilities.spiritPillCostRates[((int)tier) - 1];
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                if (qiPool.amount >= spiritCost)
                {
                    Hediff oldHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.SpiritPillHediff);
                    qiPool.amount -= spiritCost;
                    if (oldHediff != null)
                    {
                        if (oldHediff.Severity < tier)//only change if tier is higher/upgrade
                        {
                            oldHediff.Severity = 0f;//remove old hediff to add new one instead of changing severity
                                                    //why? because we want the correct offsets and this is the lazy way to do it.
                            Hediff newHediff = HediffMaker.MakeHediff(AscensionDefOf.SpiritPillHediff, pawn, null);
                            newHediff.Severity = tier;
                            pawn.health.AddHediff(newHediff, null, null, null);
                        }
                    }else
                    {
                        Hediff newHediff = HediffMaker.MakeHediff(AscensionDefOf.SpiritPillHediff, pawn, null);
                        newHediff.Severity = tier;
                        pawn.health.AddHediff(newHediff, null, null, null);
                    }
                }
                else
                {
                    SpiritKill(pawn);
                }
            }else
            {
                //explode then kill if not already dead.
                float explosionRadius = pawn.BodySize * 14;
                if (explosionRadius > 56.4)//true max is 56.4
                {
                    explosionRadius = 56.4f;
                }
                GenExplosion.DoExplosion(pawn.PositionHeld, pawn.MapHeld, explosionRadius, DamageDefOf.Bomb, pawn, -1, -1, null, null, null, null, null, 0, 0, null, false, null, 0, 0, 0, false, null, null, null, true, 1, 0f, true, null, 1f);
                pawn.Kill(null);
            }
        }
        private void SpiritKill(Pawn pawn)
        {
            //explode then kill if not already dead.
            float explosionRadius = pawn.BodySize * 14;
            if (explosionRadius > 56.4)//true max is 56.4
            {
                explosionRadius = 56.4f;
            }
            GenExplosion.DoExplosion(pawn.PositionHeld, pawn.MapHeld, explosionRadius, DamageDefOf.Bomb, pawn, -1, -1, null, null, null, null, null, 0, 0, null, false, null, 0, 0, 0, false, null, null, null, true, 1, 0f, true, null, 1f);
            pawn.Kill(null);
        }
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (parentDef.IsDrug && this.chance >= 1f)
            {
                foreach (StatDrawEntry statDrawEntry in AscensionDefOf.SpiritPillHediff.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return statDrawEntry;
                }
            }
            yield break;
        }
    }
}