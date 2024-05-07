using Verse;
using RimWorld;

namespace Ascension
{
	public class CompUseEffect_Absorb_SpiritSword : CompUseEffect
	{
		public CompProperties_UseEffect_Absorb_SpiritSword Props
		{
			get
			{
				return (CompProperties_UseEffect_Absorb_SpiritSword)this.props;
			}
		}

		//all this class does is let the pawn "absorb" the spirit sword, giving them a hediff. there might be remenants of an attempt to add abilities in the code
		public bool RecentlyFused = false;
		public override void DoEffect(Pawn usedBy)
		{
			//want to add later in case ludeon adds something of importance in it
			//base.DoEffect(usedBy);
			usedBy.health.AddHediff(AscensionDefOf.SpiritSwordFusion);

			//do this in the hediff
			//usedBy.abilities.GainAbility(AscensionDefOf.ManifestSpiritSword);
			RecentlyFused = true;
        }

        public override AcceptanceReport CanBeUsedBy(Pawn p)
        {
            if (p.health.hediffSet.HasHediff(HediffDef.Named("SpiritSwordFusion")))
            {
                return "Spirit Sword Already Fused";
            }
            return true;
        }

        public override TaggedString ConfirmMessage(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.SpiritSwordFusion, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			return null;
		}
	}
}