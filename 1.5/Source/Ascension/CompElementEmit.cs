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
    public class CompElementEmit : ThingComp
    {
        public CompProperties_ElementEmit Props => (CompProperties_ElementEmit)props;
        public int amount = 0;
        public int range = 0;
        public string element = "None";
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
            string elementText = "AS_None";
            if (Props.element == "Wood")
            {
                elementText = "AS_Wood";
            }
            else if (Props.element == "Fire")
            {
                elementText = "AS_Fire";
            }
            else if (Props.element == "Earth")
            {
                elementText = "AS_Earth";
            }
            else if (Props.element == "Metal")
            {
                elementText = "AS_Metal";
            }
            else if (Props.element == "Water")
            {
                elementText = "AS_Water";
            }
            else if (Props.element == "None")
            {
                elementText = "AS_None";
            }
            elementText = elementText.Translate();
            yield return new StatDrawEntry(
              category: AscensionDefOf.ElementEmit,
              label: "AS_ElementEmitElement".Translate(), valueString: elementText,
              reportText: "AS_ElementEmitElementDesc".Translate(),
              displayPriorityWithinCategory: 200
            );

        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            amount = Props.amount;
            range = Props.range;
            element = Props.element;
            ElementEmitMapComponent qiGatherMapComp = parent.Map.GetComponent<ElementEmitMapComponent>();
            qiGatherMapComp.AddElementAt(new IntVec2 (parent.Position.x, parent.Position.z), range, amount, GetPropsElement(element));
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PostDeSpawn(Map map)
        {
            ElementEmitMapComponent qiGatherMapComp = map.GetComponent<ElementEmitMapComponent>();
            qiGatherMapComp.RemoveElementAt(new IntVec2 (parent.Position.x, parent.Position.z), Props.range, Props.amount, GetPropsElement(Props.element));
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

        //public override void PostExposeData()
        //{
        //    base.PostExposeData();
        //    Scribe_Values.Look<int>(ref amount, "amount", Props.amount, false);
        //    Scribe_Values.Look<int>(ref range, "range", Props.range, false);
        //    Scribe_Values.Look<string>(ref element, "element", Props.element, false);
        //}
    }
}
