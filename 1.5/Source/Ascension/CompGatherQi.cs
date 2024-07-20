using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class CompGatherQi : ThingComp
    {
        public CompProperties_GatherQi Props => (CompProperties_GatherQi)props;
        public int amount = 0;
        public int range = 0;
        private QiGatherMapComponent qiGatherMapComp;
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
            amount = Props.amount * parent.stackCount;
            range = Props.range;
            qiGatherMapComp = parent.Map.GetComponent<QiGatherMapComponent>();
            qiGatherMapComp.AddQiGatherAt(parent.Position.x, parent.Position.z, Props.range, amount);//use calced amount
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            base.PreAbsorbStack(otherStack, count);
            if (otherStack != null && qiGatherMapComp != null)
            {
                int addedQiAmount = Props.amount * otherStack.stackCount;
                qiGatherMapComp.AddQiGatherAt(parent.Position.x, parent.Position.z, range, addedQiAmount);
                amount += addedQiAmount;
            }
        }
        public override void PostSplitOff(Thing piece)
        {
            base.PostSplitOff(piece);
            if (piece != null && qiGatherMapComp != null)
            {
                int removedQiAmount = Props.amount * piece.stackCount;
                qiGatherMapComp.RemoveQiGatherAt(parent.Position.x, parent.Position.z, Props.range, removedQiAmount);
                amount -= removedQiAmount;

            }
        }
        public override void PostDeSpawn(Map map)
        {
            if (map != null)
            {
                qiGatherMapComp = map.GetComponent<QiGatherMapComponent>();
                qiGatherMapComp.RemoveQiGatherAt(parent.Position.x, parent.Position.z, Props.range, amount);//why we store amount is to keep track of proper amount to remove when despawned.
            }
            base.PostDeSpawn(map);
        }
    }
}
