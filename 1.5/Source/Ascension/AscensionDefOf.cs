using RimWorld;
using UnityEngine;
using Verse;

namespace Ascension
{
    [DefOf]
    public static class AscensionDefOf
    {
        public static KeyBindingDef ToggleQiDisplay;
        public static KeyBindingDef ToggleElementDisplay;

        public static ThingDef AS_Apparel_SectUniform;

        public static HediffDef AS_GCBreakthroughHediff;

        public static IncidentDef AS_SpiritTreeConversion;

        public static EffecterDef QiMist;
        public static ThingDef Mote_QiMistA;
        public static ThingDef Mote_QiMistB;

        //misc stuffs
        public static HediffDef QiPool;
        public static HediffDef AS_SoulMend;
        public static HediffDef AS_QiResonance;
        public static HediffDef AS_HeavenlyTribulation;

        public static Verse.HediffDef AS_SpiritSwordFusion;
        public static HediffDef AS_SpiritPillHediff;
        public static HediffDef AS_QiFeed;
        public static FleckDef FlashQi;
        public static AbilityDef QiHeal;
        public static AbilityDef QiBullet;
        public static AbilityDef QiResurrection;
        public static AbilityDef ManifestSpiritSword;
        public static ThingDef AS_SpiritSword;

        public static ThingDef CultivationSpot;

        public static ThingDef AS_Plant_TreeSpirit;

        public static JobDef AS_RefineQiJob;
        public static JobDef AS_RefineQiCauldronJob;
        public static JobDef AS_QiGatheringJob;
        public static JobDef AS_ExerciseJob;

        public static JobDef AS_BreakthroughEssence;
        public static JobDef AS_BreakthroughBody;
        public static JobDef AS_GoldenCoreBreakthrough;

        public static WorkTypeDef Cultivation;

        //cultivator hidden hediff
        public static HediffDef Cultivator;

        public static StatCategoryDef GatherQi;
        public static StatCategoryDef ElementEmit;
        public static StatCategoryDef CultivationCauldron;
        public static StatCategoryDef SpiritPill;
        public static StatCategoryDef GoldenPill;
        public static StatCategoryDef CultivationUniform;

        //realm hediffs
        public static HediffDef EssenceRealm;
        public static HediffDef BodyRealm;



        //letter defs
        public static LetterDef AS_CultivationBreakthroughMessage;
        public static LetterDef AS_HeavenlyTribulationMessage;

        //static AscensionDefOf()
        //{
        //    DefOfHelper.EnsureInitializedInCtor(typeof(AscensionDefOf));
        //}
    }
}