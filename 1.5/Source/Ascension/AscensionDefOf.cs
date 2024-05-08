using RimWorld;
using Verse;

namespace Ascension
{
    [DefOf]
    public static class AscensionDefOf
    {
        //misc stuffs
        public static Verse.HediffDef SpiritSwordFusion;
        public static HediffDef QiPool;
        public static HediffDef QiFeed;
        public static FleckDef FlashQi;
        public static AbilityDef QiHeal;
        public static AbilityDef QiBullet;
        public static AbilityDef QiResurrection;
        public static AbilityDef ManifestSpiritSword;
        public static ThingDef SpiritSword;
        public static JobDef AS_BreakthroughEssence;
        public static JobDef AS_BreakthroughBody;
        public static ThingDef CultivationSpot;

        public static JobDef AS_RefineQiJob;
        public static JobDef AS_QiGatheringJob;
        public static JobDef AS_ExerciseJob;

        //cultivator hidden hediff
        public static HediffDef Cultivator;

        public static StatCategoryDef GatherQi;

        //realm hediffs
        public static HediffDef EssenceRealm;
        public static HediffDef BodyRealm;



        //letter defs
        public static LetterDef AS_CultivationBreakthroughMessage;

        //static AscensionDefOf()
        //{
        //    DefOfHelper.EnsureInitializedInCtor(typeof(AscensionDefOf));
        //}
    }
}