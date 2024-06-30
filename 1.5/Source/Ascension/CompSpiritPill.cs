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
    public class CompSpiritPill : ThingComp
    {
        float spiritCost;
        float spiritOffset;
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
            spiritCost = AscensionUtilities.spiritPillCostRates[((int)GetQualitySeverity(parent)) - 1];
            spiritOffset = AscensionUtilities.spiritPillOffsetRates[((int)GetQualitySeverity(parent)) - 1];
            return "AS_SpiritPillInspect".Translate(spiritCost.ToString().Named("COST"), spiritOffset.ToString().Named("OFFSET"));
        }
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            spiritCost = AscensionUtilities.spiritPillCostRates[((int)GetQualitySeverity(parent)) - 1];
            spiritOffset = AscensionUtilities.spiritPillOffsetRates[((int)GetQualitySeverity(parent)) - 1];

            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null)
            {
                foreach (StatDrawEntry item in enumerable)
                {
                    yield return item;
                }
            }
            yield return new StatDrawEntry(
                category: AscensionDefOf.SpiritPill,
                label: "AS_SpiritPillQiCost".Translate(), valueString: spiritCost.ToString("#"),
                reportText: "AS_SpiritPillQiCostDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.SpiritPill,
                label: "AS_SpiritPillMultiplier".Translate(), valueString: "AS_SpiritPillMultiplierValue".Translate(spiritOffset.ToString().Named("OFFSET")),
                reportText: "AS_SpiritPillMultiplierDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
        }
    }
}



