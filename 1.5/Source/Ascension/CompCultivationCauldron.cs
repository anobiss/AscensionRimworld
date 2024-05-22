

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;

namespace Ascension
{
    public class CompCultivationCauldron : ThingComp
    {
        private static readonly int[] QualityQiThresholds = { 0, 100, 250, 500, 1500, 2000, 2500 };
        public CompProperties_CultivationCauldron Props => (CompProperties_CultivationCauldron)props;
        public int currentQi = 0;
        public string translatedMaxQuality;

        public override string CompInspectStringExtra()
        {
            return "AS_CultivationCauldronInspect".Translate(Props.baseQi.ToString().Named("BASEQI"), currentQi.ToString().Named("QI"), translatedMaxQuality.Named("QUALITY"));
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
                category: AscensionDefOf.CultivationCauldron,
                label: "AS_CultivationCauldronBaseQi".Translate(), valueString: Props.baseQi.ToString(),
                reportText: "AS_CultivationCauldronBaseQiDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.CultivationCauldron,
                label: "AS_CultivationCauldronCurrentQi".Translate(), valueString: currentQi.ToString(),
                reportText: "AS_CultivationCauldronCurrentQiDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.CultivationCauldron,
                label: "AS_CultivationCauldronMaxQuality".Translate(), valueString: translatedMaxQuality,
                reportText: "AS_CultivationCauldronMaxQualityDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
        }

        public void UpdateCurrentQi()
        {
            QiGatherMapComponent qiGatherMapComp = parent.Map.GetComponent<QiGatherMapComponent>();
            int newCurrentQi = Props.baseQi;

            foreach (IntVec3 cauldronCell in parent.OccupiedRect())
            {
                newCurrentQi += qiGatherMapComp.GetQiGatherAt(cauldronCell.x, cauldronCell.z);
            }
            currentQi = newCurrentQi;
            QualityCategory maxQuality = QualityCategory.Awful;
            for (int i = QualityQiThresholds.Length - 1; i > 0; i--)
            {
                if (currentQi >= QualityQiThresholds[i])
                {
                    maxQuality = (QualityCategory)i;
                    break;
                }
            }
            translatedMaxQuality = maxQuality.GetLabel().CapitalizeFirst().ToString();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            QiGatherMapComponent qiGatherMapComp = parent.MapHeld.GetComponent<QiGatherMapComponent>();
            qiGatherMapComp.CultivationCauldrons.Add(this);
            UpdateCurrentQi();
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostDeSpawn(Map map)
        {
            QiGatherMapComponent qiGatherMapComp = parent.MapHeld.GetComponent<QiGatherMapComponent>();
            qiGatherMapComp.CultivationCauldrons.Remove(this);
            base.PostDeSpawn(map);
        }
    }
}
