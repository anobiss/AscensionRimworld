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
    public class CompLifePill : ThingComp
    {
        float lifespanAmount;
        public CompProperties_LifePill Props => (CompProperties_LifePill)props;
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
        public override string CompInspectStringExtra()
        {
            lifespanAmount = (Props.amount * AscensionUtilities.GetQualityMultiplier((int)GetQualitySeverity(parent)));

            return "AS_LifePillInspect".Translate(lifespanAmount.ToString("0.0#").Named("AMOUNT"));
        }
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            lifespanAmount = (Props.amount * AscensionUtilities.GetQualityMultiplier((int)GetQualitySeverity(parent)));

            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null)
            {
                foreach (StatDrawEntry item in enumerable)
                {
                    yield return item;
                }
            }
            yield return new StatDrawEntry(
                category: AscensionDefOf.LifePill,
                label: "AS_LifePillLifespanAmount".Translate(), valueString: "AS_LifePillYears".Translate(lifespanAmount.ToString("0.0#").Named("AMOUNT")),
                reportText: "AS_LifePillLifespanAmountDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
        }
    }
}



