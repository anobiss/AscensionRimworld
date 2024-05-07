using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;


namespace Ascension
{
    public class HediffComp_Realm : HediffComp
    {
        protected HediffCompProperties_Realm Props => (HediffCompProperties_Realm)props;

        public override void CompPostMake()
        {
            base.CompPostMake();
            //parent.LabelColor = Props.labelColor;
            //set label color
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
            {
                Find.LetterStack.ReceiveLetter(Props.letterLabel.Formatted(base.Pawn), GetLetterText(), AscensionDefOf.AS_CultivationBreakthroughMessage, base.Pawn);
            }
        }
        private TaggedString GetLetterText()
        {
            if (parent is Hediff_Pregnant { Mother: not null } hediff_Pregnant && hediff_Pregnant.Mother != hediff_Pregnant.pawn)
            {
                TaggedString result = "IvfPregnancyLetterText".Translate(hediff_Pregnant.pawn.NameFullColored);
                if (hediff_Pregnant.Mother != null && hediff_Pregnant.Father != null)
                {
                    result += "\n\n" + "IvfPregnancyLetterParents".Translate(hediff_Pregnant.Mother.NameFullColored, hediff_Pregnant.Father.NameFullColored);
                }
                return result;
            }
            return Props.letterText.Formatted(base.Pawn);
        }
    }
}