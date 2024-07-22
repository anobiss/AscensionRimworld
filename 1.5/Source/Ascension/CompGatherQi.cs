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
            amount = QiAmount(parent.stackCount);
            range = Props.range;
            qiGatherMapComp = parent.Map.GetComponent<QiGatherMapComponent>();
            if (parent != null && qiGatherMapComp != null)
            {
                qiGatherMapComp.AddQiGatherAt(parent.Position.x, parent.Position.z, range, QiAmount(parent.stackCount));
            }
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            base.PreAbsorbStack(otherStack, count);
            if (otherStack != null && qiGatherMapComp != null)
            {
                qiGatherMapComp.AddQiGatherAt(parent.Position.x, parent.Position.z, range, QiAmount(otherStack.stackCount));
            }
        }
        public override void PostSplitOff(Thing piece)//done for each piece?
        {
            base.PostSplitOff(piece);
            if (piece != null && parent.Spawned && qiGatherMapComp != null)
            {
                qiGatherMapComp.RemoveQiGatherAt(parent.Position.x, parent.Position.z, range, QiAmount(piece.stackCount));
                //Log.Message("PostSplitOff removed qi amount is" + QiAmount(piece.stackCount));
            }
        }
        public int QiAmount(int stackCount)
        {
            return Props.amount * stackCount;
        }
        public override void PostDeSpawn(Map map)
        {
            if (map != null)
            {
                qiGatherMapComp = map.GetComponent<QiGatherMapComponent>();
                if (parent != null && qiGatherMapComp != null)
                {
                    qiGatherMapComp.RemoveQiGatherAt(parent.Position.x, parent.Position.z, range, QiAmount(parent.stackCount));
                    //Log.Message("PostDeSpawn removed qi amount is" +  QiAmount(parent.stackCount));
                }
            }
            base.PostDeSpawn(map);
        }
    }
}
