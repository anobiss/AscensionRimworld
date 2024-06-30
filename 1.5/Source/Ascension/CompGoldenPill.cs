


using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static HarmonyLib.Code;

namespace Ascension
{
    public class CompGoldenPill : ThingComp
    {
        float qiAmount;
        public CompProperties_GoldenPill Props => (CompProperties_GoldenPill)props;
        public override string CompInspectStringExtra()
        {
            string goldenPillInspect;
            if (Props.noQuality == true)
            {
                goldenPillInspect = "AS_GoldenPillInspect".Translate(Props.amount.ToString().Named("QI"));
            }
            else
            {
                QualityCategory qc = new QualityCategory();
                QualityUtility.TryGetQuality(parent, out qc);
                qiAmount = (Props.amount * AscensionUtilities.GetQualityMultiplier((int)qc));
                goldenPillInspect = "AS_GoldenPillInspect".Translate(qiAmount.ToString().Named("QI"));
            }
            return goldenPillInspect;
        }
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            QualityCategory qc = new QualityCategory();
            QualityUtility.TryGetQuality(parent, out qc);
            qiAmount = (Props.amount * AscensionUtilities.GetQualityMultiplier((int)qc));

            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null)
            {
                foreach (StatDrawEntry item in enumerable)
                {
                    yield return item;
                }
            }
            yield return new StatDrawEntry(
                category: AscensionDefOf.GoldenPill,
                label: "AS_GoldenPillQi".Translate(), valueString: qiAmount.ToString("#"),
                reportText: "AS_GoldenPillQiDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
        }
    }
}



