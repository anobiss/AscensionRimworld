using Verse;
using RimWorld;

namespace Ascension
{
    public class CompUseEffect_AbilityScroll : CompUseEffect
    {
        public CompProperties_UseEffect_AbilityScroll Props
        {
            get
            {
                return (CompProperties_UseEffect_AbilityScroll)this.props;
            }
        }

        public bool RecentlyUsed = false;
        public override void DoEffect(Pawn usedBy)
        {
            Scroll_Hediff scrollHediff = HediffMaker.MakeHediff(Props.scrollHediffDef, usedBy) as Scroll_Hediff;
            HediffComp_AddScrollAbility scrollAbilityComp = scrollHediff.TryGetComp<HediffComp_AddScrollAbility>();
            usedBy.health.AddHediff(scrollHediff);
            if (scrollAbilityComp.Props.scrollCompAbilityDefList != null)
            {
                for (int i = 0; i < scrollAbilityComp.Props.scrollCompAbilityDefList.Count; i++)
                {
                    if (usedBy.abilities.GetAbility(scrollAbilityComp.Props.scrollCompAbilityDefList[i]) == null)
                    {
                        usedBy.abilities.GainAbility(scrollAbilityComp.Props.scrollCompAbilityDefList[i]);
                    }
                }
            }
        }

        public override AcceptanceReport CanBeUsedBy(Pawn p)
        {
            Scroll_Hediff scrollHediff = HediffMaker.MakeHediff(Props.scrollHediffDef, p) as Scroll_Hediff;
            Hediff essenceHediff = p.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm);
            Hediff bodyHediff = p.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm);
            HediffComp_AddScrollAbility scrollAbilityComp = scrollHediff.TryGetComp<HediffComp_AddScrollAbility>();
            HediffComp_AddScrollReq scrollReqComp = scrollHediff.TryGetComp<HediffComp_AddScrollReq>();
            if (scrollAbilityComp != null)
            {
                if (scrollAbilityComp.Props.reqEssence != 0)
                {
                    if (essenceHediff == null)
                    {
                        return "AS_ScrollHERReq".Translate()+ AscensionDefOf.EssenceRealm.stages[scrollAbilityComp.Props.reqEssence].label;
                    }else if (essenceHediff.Severity < scrollAbilityComp.Props.reqEssence)
                    {
                        return "AS_ScrollHERReq".Translate() + AscensionDefOf.EssenceRealm.stages[scrollAbilityComp.Props.reqEssence].label;
                    }
                }else if (scrollAbilityComp.Props.reqBody != 0)
                {
                    if (bodyHediff == null || bodyHediff.Severity < scrollAbilityComp.Props.reqBody)
                    {
                        return "AS_ScrollHBRReq".Translate() + AscensionDefOf.BodyRealm.stages[scrollAbilityComp.Props.reqBody].label;
                    }
                }
                if (scrollAbilityComp.Props.scrollCompAbilityDefList != null)
                {
                    for (int i = 0; i < scrollAbilityComp.Props.scrollCompAbilityDefList.Count; i++)
                    {
                        if (p.abilities.GetAbility(scrollAbilityComp.Props.scrollCompAbilityDefList[i]) != null)
                        {
                            return "AS_ScrollAL".Translate();
                        }
                    }
                }
            }
            else
            {
                Log.Message("scroll has null ability comp");
            }

            if (scrollReqComp != null)
            {
                if (scrollReqComp.Props.reqHediffsList != null)
                {
                    foreach (HediffDef reqHediffDef in scrollReqComp.Props.reqHediffsList)
                    {
                        if (p.health.hediffSet.GetFirstHediffOfDef(reqHediffDef) == null)
                        {
                            return "AS_ScrollAReq".Translate() + reqHediffDef.label;
                        }
                    }
                }
            }



            return true;
        }
        public override TaggedString ConfirmMessage(Pawn p)
        {
            //Ability firstAbilityOfDef = p.abilities.GetAbility(this.Props.abilityDef, false);
            //if (firstAbilityOfDef == null)
            //{
            //    return null;
            //}
            return null;
        }
    }
}