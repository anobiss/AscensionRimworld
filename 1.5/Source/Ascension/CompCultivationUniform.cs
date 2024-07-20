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
            string csoTranslated = "";
            string qrsoTranslated = "";
            string mqTranslated = "";
            if (Props.cultSpeedOffset > 0)
            {
                csoTranslated = "AS_CultivationUniformCSInspect".Translate((Props.cultSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("CSO"));
            }
            if (Props.qiRecSpeedOffset > 0)
            {
                qrsoTranslated = "AS_CultivationUniformQRSInspect".Translate((Props.qiRecSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("QRSO"));
            }
            if (Props.maxQiOffset > 0)
            {
                mqTranslated = "AS_CultivationUniformMQInspect".Translate((Props.maxQiOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("MQO"));
            }
            return "AS_CultivationUniformInspect".Translate(csoTranslated.Named("CS"), qrsoTranslated.Named("QRS"), mqTranslated.Named("MQ"));
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
            if (Props.cultSpeedOffset > 0f)
            {
                yield return new StatDrawEntry(
                    category: AscensionDefOf.CultivationUniform,
                    label: "AS_CultivationUniformCS".Translate(), valueString: "AS_CultivationUniformMultiplierSymbol".Translate((Props.cultSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("OFFSET")),
                    reportText: "AS_CultivationUniformCSDesc".Translate(),
                    displayPriorityWithinCategory: 200
                );
            }

            if (Props.qiRecSpeedOffset > 0f)
            {
                yield return new StatDrawEntry(
                    category: AscensionDefOf.CultivationUniform,
                    label: "AS_CultivationUniformQRS".Translate(), valueString: "AS_CultivationUniformMultiplierSymbol".Translate((Props.qiRecSpeedOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("OFFSET")),
                    reportText: "AS_CultivationUniformQRSDesc".Translate(),
                    displayPriorityWithinCategory: 200
                );
            }

            if (Props.maxQiOffset > 0f)
            {
                yield return new StatDrawEntry(
                    category: AscensionDefOf.CultivationUniform,
                    label: "AS_CultivationUniformMQ".Translate(), valueString: "AS_CultivationUniformMultiplierSymbol".Translate((Props.maxQiOffset * AscensionUtilities.GetApparelQualityMultiplier(parent as Apparel)).ToString("0.#").Named("OFFSET")),
                    reportText: "AS_CultivationUniformMQDesc".Translate(),
                    displayPriorityWithinCategory: 200
                );
            }

        }
    }
}