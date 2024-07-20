using HarmonyLib;
using RimWorld;
using Verse;
using Ascension;

namespace AscensionRoyalLib
{
    [StaticConstructorOnStartup]
    public static class AscensionRoyalLibInit
    {
        static AscensionRoyalLibInit()
        {
            var harmony = new Harmony("AscensionRoyalLib.HarmonyPatches");
            harmony.PatchAll();
            Log.Message("AscensionRoyalLib: Harmony patches applied.");
        }
    }

    [HarmonyPatch(typeof(JobDriver_Meditate), "MeditationTick")]
    public static class Ascension_MeditationHarmony
    {
        private const int ProgressInterval = 1250;

        public static void Postfix(JobDriver_Meditate __instance)
        {
            if (!ModsConfig.IsActive("Aranmaho.Xianxia"))//dont use if cultivator of the rim is active
            {
                Pawn pawn = __instance.pawn;
                //Log.Message("AscensionRoyalLib: MeditationTick called.");
                if (pawn.IsHashIntervalTick(ProgressInterval))
                {
                    //Log.Message("AscensionRoyalLib: ProgressInterval condition met.");
                    Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
                    Realm_Hediff bodyHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;

                    if (essenceHediff != null)
                    {
                        //Log.Message("AscensionRoyalLib: EssenceRealm hediff found, increasing by 0.1%.");
                        AscensionUtilities.TierProgress(pawn, AscensionDefOf.EssenceRealm, 0.001f, true);//0.1% increase

                        AscensionUtilities.TierProgress(pawn, AscensionDefOf.EssenceRealm, 1f);// + 1 increase.
                    }
                    else if (bodyHediff != null)
                    {
                        //Log.Message("AscensionRoyalLib: BodyRealm hediff found, increasing by 0.1%.");
                        AscensionUtilities.TierProgress(pawn, AscensionDefOf.BodyRealm, 0.001f, true);
                        AscensionUtilities.TierProgress(pawn, AscensionDefOf.BodyRealm, 1f);
                    }
                    else
                    {
                        //Log.Message("AscensionRoyalLib: No relevant hediff found.");
                    }
                }
                else
                {
                    //Log.Message("AscensionRoyalLib: ProgressInterval condition not met.");
                }
            }
        }
    }
}
