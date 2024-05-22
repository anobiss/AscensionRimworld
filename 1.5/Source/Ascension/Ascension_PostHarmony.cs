using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace Ascension
{
    [HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
    public class Ascension_PostHarmony
    {
        private static readonly int[] QualityQiThresholds = { 0, 100, 250, 500, 1500, 2000, 2500 };

        public static void Postfix(ref IEnumerable<Thing> __result, IBillGiver billGiver)
        {
            Thing cauldron = billGiver as Thing;
            CompCultivationCauldron cauldronComp = cauldron?.TryGetComp<CompCultivationCauldron>();
            if (cauldronComp != null)
            {
                List<Thing> modifiedResult = new List<Thing>();
                foreach (var thing in __result)
                {
                    var compQuality = thing.TryGetComp<CompQuality>();
                    if (compQuality != null)
                    {
                        QualityCategory newQuality = QualityCategory.Awful;
                        int currentQi = cauldronComp.currentQi;

                        // Determine the highest quality allowed by current Qi
                        for (int i = QualityQiThresholds.Length - 1; i > 0; i--)
                        {
                            if (currentQi >= QualityQiThresholds[i])
                            {
                                newQuality = (QualityCategory)i;
                                break;
                            }
                        }

                        // Set the quality to the highest allowed by current Qi
                        compQuality.SetQuality(newQuality, ArtGenerationContext.Colony);
                    }
                    modifiedResult.Add(thing);
                }
                __result = modifiedResult;
            }
        }
    }
}