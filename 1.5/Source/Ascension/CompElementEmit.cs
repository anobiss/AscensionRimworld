using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class CompElementEmit : ThingComp
    {
        public CompProperties_ElementEmit Props => (CompProperties_ElementEmit)props;
        public int amount = 0;
        public int range = 0;
        public string element = "None";
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
            amount = ElementAmount(parent.stackCount);
            range = Props.range;
            element = Props.element;
            qiGatherMapComp = parent.Map.GetComponent<ElementEmitMapComponent>();
            if (parent != null && qiGatherMapComp != null)
            {
                qiGatherMapComp.AddElementAt(new IntVec2(parent.Position.x, parent.Position.z), range, ElementAmount(parent.stackCount), GetPropsElement(element));
            }
            
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PreAbsorbStack(Thing otherStack, int count)
        {

            base.PreAbsorbStack(otherStack, count);
            if (otherStack != null && qiGatherMapComp != null)
            {
                qiGatherMapComp.AddElementAt(new IntVec2(parent.Position.x, parent.Position.z), range, ElementAmount(otherStack.stackCount), GetPropsElement(element));
            }
        }

        public override void PostSplitOff(Thing piece)
        {
            base.PostSplitOff(piece);
            if (piece != null && parent.Spawned && qiGatherMapComp != null)
            {
                qiGatherMapComp.RemoveElementAt(new IntVec2(parent.Position.x, parent.Position.z), range, ElementAmount(piece.stackCount), GetPropsElement(Props.element));
                //Log.Message("PostSplitOff removed element amount is" + ElementAmount(piece.stackCount));
            }
        }
        public int ElementAmount(int stackCount)
        {
            return Props.amount * stackCount;
        }

        public override void PostDeSpawn(Map map)
        {
            if (map != null)
            {
                qiGatherMapComp = map.GetComponent<ElementEmitMapComponent>();
                if (parent != null && qiGatherMapComp != null)
                {
                    qiGatherMapComp.RemoveElementAt(new IntVec2(parent.Position.x, parent.Position.z), range, ElementAmount(parent.stackCount), GetPropsElement(Props.element));
                    //Log.Message("PostDeSpawn removed element amount is" + ElementAmount(parent.stackCount));
                }
            }
            
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


