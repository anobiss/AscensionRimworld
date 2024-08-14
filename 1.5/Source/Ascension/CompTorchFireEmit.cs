using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class CompTorchFireEmit : ThingComp
    {
        public CompProperties_TorchFireEmit Props => (CompProperties_TorchFireEmit)props;
        private ElementEmitMapComponent.Element element = ElementEmitMapComponent.Element.Fire;
        private ElementEmitMapComponent qiGatherMapComp;
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
                category: AscensionDefOf.ElementEmit,
                label: "AS_ElementEmitAmount".Translate(), valueString: Props.amount.ToString(),
                reportText: "AS_ElementEmitAmountTorchDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.ElementEmit,
                label: "AS_ElementEmitRange".Translate(), valueString: Props.range.ToString(),
                reportText: "AS_ElementEmitRangeDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            string elementText = "AS_Fire";
            elementText = elementText.Translate();
            yield return new StatDrawEntry(
              category: AscensionDefOf.ElementEmit,
              label: "AS_ElementEmitElement".Translate(), valueString: elementText,
              reportText: "AS_ElementEmitElementDesc".Translate(),
              displayPriorityWithinCategory: 200
            );

        }
        private CompRefuelable torchFuelComp;
        bool addedFire = false;
        int ticks;
        private static readonly int tickRate = 1400;
        public override void CompTick()
        {
            if (qiGatherMapComp != null)
            {
                if (torchFuelComp != null)
                {
                    ticks--;
                    if (ticks <= 0)
                    {
                        ticks = tickRate;
                        qiGatherMapComp.UpdateMapElement();
                    }
                }
            }

        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            torchFuelComp = parent.TryGetComp<CompRefuelable>();
            //find and assign thier comp
            qiGatherMapComp = parent.Map.GetComponent<ElementEmitMapComponent>();
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PostDeSpawn(Map map)
        {
            if (qiGatherMapComp != null)
            {
                qiGatherMapComp.UpdateMapElement();
            }
            base.PostDeSpawn(map);
        }
    }
}
