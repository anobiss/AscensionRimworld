using KTrie;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Ascension
{
    public class IncidentWorker_SpiritTreeConversion : IncidentWorker
    {
        public Thing GetRandomTree(Map map)
        {
            List<Thing> trees = new List<Thing>();

            bool treeReqFlag = false;

            foreach (Thing thing in map.listerThings.AllThings)
            {
                
                if (thing.def.plant != null && thing.def.plant.IsTree)
                {
                    treeReqFlag = true;
                    
                }
                if (ModLister.RoyaltyInstalled)
                {
                    if (thing.def == ThingDefOf.Plant_TreeAnima)
                    {
                        treeReqFlag = false;
                    }
                }
                if (treeReqFlag == true)
                {
                    trees.Add(thing);
                }
            }

            if (trees.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(trees.Count);
                return trees[index];
            }
            else
            {
                return null;
            }
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;

            Thing targetTree = GetRandomTree(map);
            if (targetTree != null)
            {
                Thing newSpiritTree = ThingMaker.MakeThing(AscensionDefOf.AS_Plant_TreeSpirit, null); ;
                GenPlace.TryPlaceThing(newSpiritTree, targetTree.PositionHeld, targetTree.MapHeld, ThingPlaceMode.Near, out Thing t, null, null, default(Rot4));
                targetTree.Destroy();
                SendStandardLetter("AS_SpiritTreeConversion".Translate(), "AS_SpiritTreeConversionDesc".Translate(), LetterDefOf.PositiveEvent, parms, newSpiritTree);

            }else
            {
                return false;
            }
            return true;
        }
    }
}
