using Verse;

namespace Ascension
{
    public class AS_DamageWorker_StunSeed : DamageWorker
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            DamageResult damageResult = base.Apply(dinfo, victim);
            damageResult.stunned = true;
            Pawn victimPawn = victim as Pawn;
            if (victimPawn != null)
            {
                victimPawn.health.AddHediff(AscensionDefOf.AS_StunSeedHediff, victimPawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth));
            }
            return damageResult;
        }
    }
}