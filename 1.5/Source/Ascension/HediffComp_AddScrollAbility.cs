using RimWorld;
using System.Collections.Generic;
using Verse;


namespace Ascension
{
    public class HediffComp_AddScrollAbility : HediffComp
    {
        public HediffCompProperties_AddScrollAbility Props => (HediffCompProperties_AddScrollAbility)props;
        public override void CompPostPostAdd(DamageInfo? dinfo)//do abilities post add so we can check stuff without giving abilities and only give abilites on add hediff
        {
            base.CompPostPostAdd(dinfo);
            Scroll_Hediff scrollHediff = this.parent as Scroll_Hediff;
            if (scrollHediff != null)
            {
                AddScrollAbilities();
            }
        }
        public void AddScrollAbilities()
        {
            Scroll_Hediff scrollHediff = this.parent as Scroll_Hediff;
            if (scrollHediff != null)
            {
                if (Pawn != null)
                {
                    foreach (AbilityDef ability in Props.scrollCompAbilityDefList)//gives the scrolls abilities
                    {
                        if (ability != null)
                        {
                            if (Pawn.abilities != null)
                            {
                                Pawn.abilities.GainAbility(ability);
                            }else
                            {
                                //Log.Message("tracker null--------------------------");
                            }
                            //Log.Message("adding ability" + ability.label);
                        }else
                        {
                            //Log.Message("ability null--------------------------");
                        }
                    }
                }else
                {
                    //Log.Message("ascension error pawn null");
                }
            }
        }
    }
}