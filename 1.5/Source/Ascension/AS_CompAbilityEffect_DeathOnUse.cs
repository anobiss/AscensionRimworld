using RimWorld;
using Verse;
namespace Ascension
{
    public class AS_CompAbilityEffect_DeathOnUse : CompAbilityEffect
    {
        public new AS_CompProperties_DeathOnUse Props => (AS_CompProperties_DeathOnUse)props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = parent.pawn;
            //Log.Message("killing entombomber");
            if (pawn != null)
            {
                if (Props.destroy == true)
                {
                    pawn.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    pawn.Kill(null);
                    //Log.Message("killed entombomber");
                }
            }
        }
    }
}
