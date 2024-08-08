
using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using static HarmonyLib.Code;

namespace Ascension
{
    public class IngestionOutcomeDoer_RebirthPill : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Realm_Hediff bodyRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
            Realm_Hediff essenceRealm = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                pawn.health.RemoveHediff(cultivatorHediff);
            }

            if (qiPool != null)
            {
                pawn.health.RemoveHediff(qiPool);
            }
            if (bodyRealm != null)
            {
                pawn.health.RemoveHediff(bodyRealm);
            }
            if (essenceRealm != null)
            {
                pawn.health.RemoveHediff(essenceRealm);
            }

            pawn.health.AddHediff(AscensionDefOf.Cultivator);
            qiPool = HediffMaker.MakeHediff(AscensionDefOf.QiPool, pawn, null) as QiPool_Hediff;
            qiPool.amount = 1;
            pawn.health.AddHediff(qiPool);
            AscensionUtilities.UpdateQiMax(qiPool);
        }
    }
}