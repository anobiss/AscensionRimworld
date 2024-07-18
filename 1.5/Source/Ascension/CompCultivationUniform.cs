using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class CompCultivationUniform : ThingComp
    {
        public CompProperties_CultivationUniform Props => (CompProperties_CultivationUniform)props;


        //display bonuses
        public override string CompInspectStringExtra()
        {
            return "AS_CultivationUniformInspect".Translate((Props.cultSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("CSO"), (Props.qiRecSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("QRSO"));
        }
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null)
            {
                foreach (StatDrawEntry item in enumerable)
                {
                    yield return item;
                }
            }
            yield return new StatDrawEntry(
                category: AscensionDefOf.CultivationUniform,
                label: "AS_CultivationUniformCS".Translate(), valueString: "AS_CultivationUniformMultiplierSymbol".Translate((Props.cultSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("OFFSET")),
                reportText: "AS_CultivationUniformCSDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.CultivationUniform,
                label: "AS_CultivationUniformQRS".Translate(), valueString: "AS_CultivationUniformMultiplierSymbol".Translate((Props.qiRecSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("OFFSET")),
                reportText: "AS_CultivationUniformQRSDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
        }
    }
}