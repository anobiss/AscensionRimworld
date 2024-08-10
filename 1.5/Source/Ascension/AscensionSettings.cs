using System.Collections.Generic;
using Verse;
using UnityEngine;
using System;
using System.Reflection.Emit;
using System.Runtime;

namespace Ascension
{
    public class AscensionSettings : ModSettings
    {
        public static readonly float SMTickRateDefault = 100f; //soul mend tick rate, if 0 do every tick
        public static readonly float LifespanTickRateDefault = 1200f;
        public static readonly float LifespanAgeRatioDefault = 0.15f;

        public static readonly float MaxRandLifespanDefault = 0.5f;


        public static readonly bool displayQiGridDefault = true;
        public static readonly bool displayElementGridDefault = true;
        public static readonly bool humanoidOnlyBoolDefault = false;
        public static readonly bool logHealsBoolDefault = false;
        public static readonly bool opGoldenPillsBoolDefault = false;
        public static readonly bool machineCultivatorBoolDefault = false;//psuedo immortality default chance must be way lower than foundation chance, as psuedo immortality and beyond are considered as heavy time and resource investments.

        public static readonly float GoldenCoreMaxDefault = 5000f;
        public static readonly float AnimaCMaxDefault = 5000f;

        public static readonly float CultivatorChanceDefault = 0.1f;

        public static readonly float AbilityChanceDefault = 0.2f;

        public static readonly float PCChanceDefault = 0.1f;//powerful cultivator, always past 3rd realm and 4x scores. 
        public static readonly float PCMinRealmDefault = 3f;
        public static readonly float PCMaxRealmDefault = 6f;


        public static readonly float EssenceChanceDefault = 0.1f;//chance for randomly generated cultivators to be essence cultivators instead of body cultivators if past foundation. do 0% for only body 
        public static readonly float foundationChanceDefault = 0.2f;
        public static readonly bool disableProgressLettersDefault = true;
        public static readonly bool disableHTLettersDefault = false;
        public static readonly float SMRandStacksDefault = 12f;


        public static readonly float BaseCultivationSpeedDefault = 1f;

        //all controllable pawns auto choose by default if this is true;
        public static readonly bool defaultConfirmDefault = false;

        public static readonly bool defaultEssenceTypeDefault = true;//colonist cultivation type is essence by defaut, body if disabled





        public float SMTickRate = SMTickRateDefault; //soul mend tick rate, if 0 do every tick
        public string SMTickRateString = SMTickRateDefault.ToString("#");
        public float LifespanTickRate = LifespanTickRateDefault;
        public string LifespanTickRateString = LifespanTickRateDefault.ToString("#");
        public float LifespanAgeRatio = LifespanAgeRatioDefault;
        public string LifespanAgeRatioString = LifespanAgeRatioDefault.ToString("0.0#");

        public float MaxRandLifespan = MaxRandLifespanDefault;
        public string MaxRandLifespanString = MaxRandLifespanDefault.ToString("0.0#");


        public bool displayQiGrid = displayQiGridDefault;
        public bool displayElementGrid = displayElementGridDefault;
        public bool humanoidOnlyBool = humanoidOnlyBoolDefault;
        public bool logHealsBool = logHealsBoolDefault;
        public bool opGoldenPillsBool = opGoldenPillsBoolDefault;
        public bool machineCultivatorBool = machineCultivatorBoolDefault;//psuedo immortality default chance must be way lower than foundation chance, as psuedo immortality and beyond are considered as heavy time and resource investments.

        public float GoldenCoreMax = GoldenCoreMaxDefault;
        public string GoldenCoreMaxString = GoldenCoreMaxDefault.ToString("#");
        public float AnimaCMax = AnimaCMaxDefault;
        public string AnimaCMaxString = AnimaCMaxDefault.ToString("#");

        public float CultivatorChance = CultivatorChanceDefault;
        public string CultivatorChanceString = CultivatorChanceDefault.ToString("0.0#");

        public float AbilityChance = AbilityChanceDefault;
        public string AbilityChanceString = AbilityChanceDefault.ToString("0.0#");

        public float PCChance = PCChanceDefault;//powerful cultivator, always past 3rd realm and 4x scores. 
        public string PCChanceString = PCChanceDefault.ToString("0.0#");
        public float PCMinRealm = PCMinRealmDefault;
        public string PCMinRealmString = PCMinRealmDefault.ToString("#");
        public float PCMaxRealm = PCMaxRealmDefault;
        public string PCMaxRealmString = PCMaxRealmDefault.ToString("#");


        public float EssenceChance = EssenceChanceDefault;//chance for randomly generated cultivators to be essence cultivators instead of body cultivators if past foundation. do 0% for only body 
        public string EssenceChanceString = EssenceChanceDefault.ToString("0.0#");
        public float foundationChance = foundationChanceDefault;
        public string foundationChanceString = foundationChanceDefault.ToString("0.0#");
        public bool disableProgressLetters = disableProgressLettersDefault;
        public bool disableHTLetters = disableHTLettersDefault;
        public float SMRandStacks = SMRandStacksDefault;
        public string SMRandStacksString = SMRandStacksDefault.ToString("#");


        public float BaseCultivationSpeed = BaseCultivationSpeedDefault;
        public string BaseCultivationSpeedString = BaseCultivationSpeedDefault.ToString("0.0#");

        //all controllable pawns auto choose by default if this is true;
        public bool defaultConfirm = defaultConfirmDefault;

        public bool defaultEssenceType = defaultEssenceTypeDefault;//colonist cultivation type is essence by defaut, body if disabled



        //all controllable pawns auto choose any specific realm specified

        public override void ExposeData()
        {
            
            Scribe_Values.Look(ref LifespanAgeRatio, "LifespanAgeRatio", 0.15f);
            Scribe_Values.Look(ref BaseCultivationSpeed, "BaseCultivationSpeed", 1f);
            Scribe_Values.Look(ref foundationChance, "foundationChance", 0.2f);
            Scribe_Values.Look(ref defaultConfirm, "defaultConfirm", false);
            Scribe_Values.Look(ref defaultEssenceType, "defaultEssenceType", true);
            Scribe_Values.Look(ref disableHTLetters, "disableHTLetters", false);
            Scribe_Values.Look(ref disableProgressLetters, "disableProgressLetters", true);
            Scribe_Values.Look(ref displayQiGrid, "displayQiGrid", true);
            Scribe_Values.Look(ref displayElementGrid, "displayElementGrid", true);
            Scribe_Values.Look(ref humanoidOnlyBool, "humanoidOnlyBool");
            Scribe_Values.Look(ref machineCultivatorBool, "machineCultivatorBool");
            Scribe_Values.Look(ref opGoldenPillsBool, "opGoldenPillsBool");
            Scribe_Values.Look(ref logHealsBool, "logHealsBool");
            Scribe_Values.Look(ref CultivatorChance, "CultivatorChance", 0.1f);
            Scribe_Values.Look(ref GoldenCoreMax, "GoldenCoreMax", 5000f);
            Scribe_Values.Look(ref AnimaCMax, "AnimaCMax", 5000f);
            Scribe_Values.Look(ref EssenceChance, "EssenceChance", 0.1f);

            Scribe_Values.Look(ref PCChance, "PCChance", 0.1f);
            Scribe_Values.Look(ref PCMinRealm, "PCMinRealm", 3f);
            Scribe_Values.Look(ref PCMaxRealm, "PCMaxRealm", 6f);

            Scribe_Values.Look(ref AbilityChance, "AbilityChance", 0.2f);

            Scribe_Values.Look(ref SMTickRate, "SMTickRate", 100f);
            Scribe_Values.Look(ref LifespanTickRate, "LifespanTickRate", 1200f);
            Scribe_Values.Look(ref SMRandStacks, "SMRandStacks", 12f);
            Scribe_Values.Look(ref MaxRandLifespan, "MaxRandLifespan", 0.5f);
            base.ExposeData();
        }
    }

    public class AscensionMod : Mod
    {
        AscensionSettings settings;
        private Vector2 scrollPosition = Vector2.zero;

        public AscensionMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<AscensionSettings>();
        }

        private static string SettingsChanceString(float chance, bool isLabel = true)
        {
            if (isLabel == true)
            {
                return (chance * 100f).ToString("0.0#");
            }
            else
            {
                return (chance).ToString("0.0#");
            }

        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            Rect outRect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, 1200f); // Adjust height as needed

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            listingStandard.Begin(viewRect);
            if (listingStandard.ButtonText("AS_ResetSettings".Translate()))
            {
                AllDefault(settings);
            }
            listingStandard.Label("AS_RandomCultivators".Translate()+ "\n", -1, "AS_RandomCultivatorsDesc".Translate());

            listingStandard.CheckboxLabeled("AS_DefaultEssenceType".Translate(), ref settings.defaultEssenceType, "AS_DefaultEssenceTypeDesc".Translate());
            listingStandard.CheckboxLabeled("AS_DefaultConfirm".Translate(), ref settings.defaultConfirm, "AS_DefaultConfirmDesc".Translate());
            listingStandard.CheckboxLabeled("AS_HumanoidCultivator".Translate(), ref settings.humanoidOnlyBool, "AS_HumanoidCultivatorDesc".Translate());
            listingStandard.CheckboxLabeled("AS_MachineCultivator".Translate(), ref settings.machineCultivatorBool, "AS_MachineCultivatorDesc".Translate());

            listingStandard.Label("AS_CultivatorChance".Translate(SettingsChanceString(settings.CultivatorChance).Named("SETTING"), -1, "AS_CultivatorChanceDesc".Translate()));
            settings.CultivatorChanceString = SettingsChanceString(settings.CultivatorChance, false);
            listingStandard.TextFieldNumeric(ref settings.CultivatorChance, ref settings.CultivatorChanceString, 0.00f, 1.00f);
            listingStandard.Label("AS_FoundationChance".Translate(SettingsChanceString(settings.foundationChance).Named("SETTING"), -1, "AS_FoundationChanceDesc".Translate()));
            settings.foundationChanceString = SettingsChanceString(settings.foundationChance, false);
            listingStandard.TextFieldNumeric(ref settings.foundationChance, ref settings.foundationChanceString, 0.00f, 1.00f);
            listingStandard.Label("AS_ERChance".Translate(SettingsChanceString(settings.EssenceChance).Named("SETTING"), -1, "AS_ERChanceDesc".Translate()));
            settings.EssenceChanceString = SettingsChanceString(settings.EssenceChance, false);
            listingStandard.TextFieldNumeric(ref settings.EssenceChance, ref settings.EssenceChanceString, 0.00f, 1.00f);

            listingStandard.Label("AS_PCChance".Translate(SettingsChanceString(settings.PCChance).Named("SETTING"), -1, "AS_PCChanceDesc".Translate()));

            settings.PCChanceString = SettingsChanceString(settings.PCChance, false);
            listingStandard.TextFieldNumeric(ref settings.PCChance, ref settings.PCChanceString, 0.00f, 1.00f);

            listingStandard.Label("AS_PCMin".Translate(this.settings.PCMinRealm.ToString("#").Named("SETTING")), -1, "AS_PCMinDesc".Translate());
            settings.PCMinRealmString = settings.PCMinRealm.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.PCMinRealm, ref settings.PCMinRealmString, 1, settings.PCMaxRealm);

            listingStandard.Label("AS_PCMax".Translate(this.settings.PCMaxRealm.ToString("#").Named("SETTING")), -1, "AS_PCMaxDesc".Translate());
            settings.PCMaxRealmString = settings.PCMaxRealm.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.PCMaxRealm, ref settings.PCMaxRealmString, (float)Math.Floor(settings.PCMinRealm), 7);

            listingStandard.Label("AS_GoldenCoreMax".Translate(this.settings.GoldenCoreMax.ToString("#").Named("SETTING")), -1, "AS_GoldenCoreMaxDesc".Translate());
            settings.GoldenCoreMaxString = settings.GoldenCoreMax.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.GoldenCoreMax, ref settings.GoldenCoreMaxString, 1, 1000000f);

            listingStandard.Label("AS_AnimaCMax".Translate(this.settings.AnimaCMax.ToString("#").Named("SETTING")), -1, "AS_AnimaCMaxDesc".Translate());
            settings.AnimaCMaxString = settings.AnimaCMax.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.AnimaCMax, ref settings.AnimaCMaxString, 1, 1000000f);

            listingStandard.Label("AS_AbilityChance".Translate(SettingsChanceString(settings.AbilityChance).Named("SETTING"), -1, "AS_AbilityChanceDesc".Translate()));
            settings.AbilityChanceString = SettingsChanceString(settings.AbilityChance, false);
            listingStandard.TextFieldNumeric(ref settings.AbilityChance, ref settings.AbilityChanceString, 0.00f, 1.00f);

            listingStandard.Label("AS_SMRandStacks".Translate(this.settings.SMRandStacks.ToString("#").Named("SETTING")), -1, "AS_SSMRandStacksDesc".Translate());
            settings.SMRandStacksString = settings.SMRandStacks.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.SMRandStacks, ref settings.SMRandStacksString, 1, 100000f);

            listingStandard.Label("AS_MaxRandLifespan".Translate(SettingsChanceString(settings.MaxRandLifespan).Named("SETTING")), -1, "AS_MaxRandLifespanDesc".Translate());
            settings.MaxRandLifespanString = SettingsChanceString(settings.MaxRandLifespan, false);
            listingStandard.TextFieldNumeric(ref settings.MaxRandLifespan, ref settings.MaxRandLifespanString, 0.00f, 100000.00f);

            listingStandard.Label("AS_Notifications".Translate() + "\n", -1, "AS_NotificationsDesc".Translate());

            listingStandard.CheckboxLabeled("AS_LogHeals".Translate(), ref settings.logHealsBool, "AS_LogHealsDesc".Translate());
            listingStandard.CheckboxLabeled("AS_DisableHTLetters".Translate(), ref settings.disableHTLetters, "AS_DisableHTLettersDesc".Translate());
            listingStandard.CheckboxLabeled("AS_DisablePLetters".Translate(), ref settings.disableProgressLetters, "AS_DisablePLettersDesc".Translate());

            listingStandard.Label("AS_TickRates".Translate() + "\n", -1, "AS_TickRatesDesc".Translate());

            listingStandard.Label("AS_LifespanTickRate".Translate(this.settings.LifespanTickRate.ToString("#").Named("SETTING")), -1, "AS_LifespanTickRateDesc".Translate());
            settings.LifespanTickRateString = settings.LifespanTickRate.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.LifespanTickRate, ref settings.LifespanTickRateString, 1, 100000f);


            listingStandard.Label("AS_SMTickRate".Translate(this.settings.SMTickRate.ToString("#").Named("SETTING")),-1, "AS_SMTickRateDesc".Translate());

            settings.SMTickRateString = settings.SMTickRate.ToString("#");
            listingStandard.TextFieldNumeric(ref settings.SMTickRate, ref settings.SMTickRateString, 1, 100000f);


            listingStandard.Label("AS_Misc".Translate() + "\n", -1, "AS_MiscDesc".Translate());

            listingStandard.CheckboxLabeled("AS_DisplayQi".Translate(), ref settings.displayQiGrid, "AS_DisplayQiDesc".Translate());
            listingStandard.CheckboxLabeled("AS_DisplayElement".Translate(), ref settings.displayElementGrid, "AS_DisplayElementDesc".Translate());
            listingStandard.Label("AS_BaseCultivationSpeed".Translate(this.settings.BaseCultivationSpeed.ToString("#").Named("SETTING")), -1, "AS_BaseCultivationSpeedDesc".Translate());
            settings.BaseCultivationSpeedString = settings.BaseCultivationSpeed.ToString("0.0#");
            listingStandard.TextFieldNumeric(ref settings.BaseCultivationSpeed, ref settings.BaseCultivationSpeedString, 1, 10000f);

            listingStandard.Label("AS_LifespanAgeRatio".Translate(SettingsChanceString(settings.LifespanAgeRatio).Named("SETTING"), -1, "AS_LifespanAgeRatioDesc".Translate()));
            settings.LifespanAgeRatioString = SettingsChanceString(settings.LifespanAgeRatio, false);
            listingStandard.TextFieldNumeric(ref settings.LifespanAgeRatio, ref settings.LifespanAgeRatioString, 0.00f, 1.00f);



            listingStandard.End();
            Widgets.EndScrollView();
            base.DoSettingsWindowContents(inRect);
        }

        private static void AllDefault(AscensionSettings settings)
        {
            settings.SMTickRate = AscensionSettings.SMTickRateDefault;
            settings.SMTickRateString = AscensionSettings.SMTickRateDefault.ToString("#");
            settings.LifespanTickRate = AscensionSettings.LifespanTickRateDefault;
            settings.LifespanTickRateString = AscensionSettings.LifespanTickRateDefault.ToString("#");
            settings.LifespanAgeRatio = AscensionSettings.LifespanAgeRatioDefault;
            settings.LifespanAgeRatioString = AscensionSettings.LifespanAgeRatioDefault.ToString("0.0#");
            settings.MaxRandLifespan = AscensionSettings.MaxRandLifespanDefault;
            settings.MaxRandLifespanString = AscensionSettings.MaxRandLifespanDefault.ToString("0.0#");
            settings.displayQiGrid = AscensionSettings.displayQiGridDefault;
            settings.displayElementGrid = AscensionSettings.displayElementGridDefault;
            settings.humanoidOnlyBool = AscensionSettings.humanoidOnlyBoolDefault;
            settings.logHealsBool = AscensionSettings.logHealsBoolDefault;
            settings.opGoldenPillsBool = AscensionSettings.opGoldenPillsBoolDefault;
            settings.machineCultivatorBool = AscensionSettings.machineCultivatorBoolDefault;
            settings.GoldenCoreMax = AscensionSettings.GoldenCoreMaxDefault;
            settings.GoldenCoreMaxString = AscensionSettings.GoldenCoreMaxDefault.ToString("#");
            settings.AnimaCMax = AscensionSettings.AnimaCMaxDefault;
            settings.AnimaCMaxString = AscensionSettings.AnimaCMaxDefault.ToString("#");
            settings.CultivatorChance = AscensionSettings.CultivatorChanceDefault;
            settings.CultivatorChanceString = AscensionSettings.CultivatorChanceDefault.ToString("0.0#");
            settings.AbilityChance = AscensionSettings.AbilityChanceDefault;
            settings.AbilityChanceString = AscensionSettings.AbilityChanceDefault.ToString("0.0#");
            settings.PCChance = AscensionSettings.PCChanceDefault;
            settings.PCChanceString = AscensionSettings.PCChanceDefault.ToString("0.0#");
            settings.PCMinRealm = AscensionSettings.PCMinRealmDefault;
            settings.PCMinRealmString = AscensionSettings.PCMinRealmDefault.ToString("#");
            settings.PCMaxRealm = AscensionSettings.PCMaxRealmDefault;
            settings.PCMaxRealmString = AscensionSettings.PCMaxRealmDefault.ToString("#");
            settings.EssenceChance = AscensionSettings.EssenceChanceDefault;
            settings.EssenceChanceString = AscensionSettings.EssenceChanceDefault.ToString("0.0#");
            settings.foundationChance = AscensionSettings.foundationChanceDefault;
            settings.foundationChanceString = AscensionSettings.foundationChanceDefault.ToString("0.0#");
            settings.disableProgressLetters = AscensionSettings.disableProgressLettersDefault;
            settings.disableHTLetters = AscensionSettings.disableHTLettersDefault;
            settings.SMRandStacks = AscensionSettings.SMRandStacksDefault;
            settings.SMRandStacksString = AscensionSettings.SMRandStacksDefault.ToString("#");
            settings.BaseCultivationSpeed = AscensionSettings.BaseCultivationSpeedDefault;
            settings.BaseCultivationSpeedString = AscensionSettings.BaseCultivationSpeedDefault.ToString("0.0#");
            settings.defaultConfirm = AscensionSettings.defaultConfirmDefault;
            settings.defaultEssenceType = AscensionSettings.defaultEssenceTypeDefault;
    }

        public override string SettingsCategory()
        {
            return "AS_Name".Translate();
        }
    }
}
