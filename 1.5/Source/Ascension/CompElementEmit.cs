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
    public class CompElementEmit : ThingComp
    {
        public CompProperties_ElementEmit Props => (CompProperties_ElementEmit)props;


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
                reportText: "AS_ElementEmitAmountDesc".Translate(),
                displayPriorityWithinCategory: 200
            );
            yield return new StatDrawEntry(
                category: AscensionDefOf.ElementEmit,
                label: "AS_ElementEmitRange".Translate(), valueString: Props.range.ToString(),
                reportText: "AS_ElementEmitRangeDesc".Translate(),
                displayPriorityWithinCategory: 200
            );


        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {

            ElementEmitMapComponent qiGatherMapComp = parent.Map.GetComponent<ElementEmitMapComponent>();
            qiGatherMapComp.AddElementEmitAt(parent.Position.x, parent.Position.z, Props.range, Props.amount, GetPropsElement(Props.element));
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PostDeSpawn(Map map)
        {
            ElementEmitMapComponent qiGatherMapComp = map.GetComponent<ElementEmitMapComponent>();
            qiGatherMapComp.RemoveElementEmitAt(parent.Position.x, parent.Position.z, Props.range, Props.amount, GetPropsElement(Props.element));
            base.PostDeSpawn(map);
        }
        private static ElementEmitMapComponent.Element GetPropsElement(string elementText)
        {
            ElementEmitMapComponent.Element element = ElementEmitMapComponent.Element.None;
            if (elementText == "Metal")
            {
                element = ElementEmitMapComponent.Element.Metal;
            }
            else if (elementText == "Water")
            {
                element = ElementEmitMapComponent.Element.Water;
            }
            else if (elementText == "Wood")
            {
                element = ElementEmitMapComponent.Element.Wood;
            }
            else if (elementText == "Fire")
            {
                element = ElementEmitMapComponent.Element.Fire;
            }
            else if (elementText == "Earth")
            {
                element = ElementEmitMapComponent.Element.Earth;
            }
            return element;
        }
    }
}
