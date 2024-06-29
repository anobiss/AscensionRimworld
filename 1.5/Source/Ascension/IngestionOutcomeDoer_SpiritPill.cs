using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
namespace Ascension
{
    public class IngestionOutcomeDoer_SpiritPill : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                if (qiPool.amount >= 100000)
                {
                    Hediff hediff = HediffMaker.MakeHediff(AscensionDefOf.SpiritPillHediff, pawn, null);
                    pawn.health.AddHediff(hediff, null, null, null);
                }
                else
                {
                    //explode then kill if not already dead.
                    float explosionRadius = pawn.BodySize*14;
                    if (explosionRadius > 56.4)//true max is 56.4
                    {
                        explosionRadius = 56.4f;
                    }
                    GenExplosion.DoExplosion(pawn.PositionHeld, pawn.MapHeld, explosionRadius, DamageDefOf.Bomb, pawn, -1, -1, null, null, null, null, null, 0, 0, null, false, null, 0, 0, 0, false, null, null, null, true, 1, 0f, true, null, 1f);
                    pawn.Kill(null);
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