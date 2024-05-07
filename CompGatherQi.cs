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
    public class CompGatherQi : ThingComp
    {
        public CompProperties_GatherQi Props => (CompProperties_GatherQi)props;


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
                category: AscensionDefOf.GatherQi,
                label: "AS_GatherQiAmount".Translate(), valueString: Props.amount.ToString(),
                reportText: "AS_GatherQiAmountDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.GatherQi,
                label: "AS_GatherQiRange".Translate(), valueString: Props.range.ToString(),
                reportText: "AS_GatherQiRangeDesc".Translate(),
                displayPriorityWithinCategory: 200
            );


        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            QiGatherMapComponent qiGatherMapComp = parent.Map.GetComponent<QiGatherMapComponent>();
            qiGatherMapComp.AddQiGatherAt(parent.Position.x, parent.Position.y, Props.range, Props.amount);
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PostDeSpawn(Map map)
        {
            QiGatherMapComponent qiGatherMapComp = map.GetComponent<QiGatherMapComponent>();
            qiGatherMapComp.RemoveQiGatherAt(parent.Position.x, parent.Position.y, Props.range, Props.amount);
            base.PostDeSpawn(map);
        }
    }
}
