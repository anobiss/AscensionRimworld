using System.Collections.Generic;
using Verse;

namespace Ascension
{
    [StaticConstructorOnStartup]
    public static class AscensionStaticStartUtils
    {
        private static readonly HashSet<HediffDef> scrollHediffList = new HashSet<HediffDef>();

        public static IReadOnlyCollection<HediffDef> ScrollHediffList
        {
            get { return scrollHediffList; }
        }

        static AscensionStaticStartUtils()
        {
            foreach (HediffDef def in DefDatabase<HediffDef>.AllDefsListForReading)
            {
                if (def.HasComp(typeof(HediffComp_AddScrollAbility)))
                {
                    scrollHediffList.Add(def);
                    //Log.Message("1 scroll hediff added");
                }
            }
        }
    }
}