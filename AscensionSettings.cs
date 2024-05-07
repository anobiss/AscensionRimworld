using System.Collections.Generic;
using Verse;
using UnityEngine;
using System;
using System.Reflection.Emit;

namespace Ascension
{
    public class AscensionSettings : ModSettings
    {
        /// <summary>
        /// Default settings.
        /// </summary>
        public bool humanoidOnlyBool = false;
        public bool logHealsBool = false;
        public bool opGoldenPillsBool = false;
        public bool machineCultivatorBool = false;
        //psuedo immortality default chance must be way lower than foundation chance, as psuedo immortality and beyond are considered as heavy time and resource investments.
        public float CultivatorChance = 0.1f;
        public float PIChance = 0.01f;
        public float AbilityChance = 0.1f;
        public float PCChance = 0.1f;
        public float EssenceChance = 0.1f;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref humanoidOnlyBool, "humanoidOnlyBool");
            Scribe_Values.Look(ref machineCultivatorBool, "machineCultivatorBool");
            Scribe_Values.Look(ref opGoldenPillsBool, "opGoldenPillsBool");
            Scribe_Values.Look(ref logHealsBool, "logHealsBool");
            Scribe_Values.Look(ref CultivatorChance, "CultivatorChance", 0.1f);
            Scribe_Values.Look(ref EssenceChance, "EssenceChance", 0.1f);
            Scribe_Values.Look(ref PIChance, "PIChance", 0.1f);
            Scribe_Values.Look(ref AbilityChance, "AbilityChance", 0.2f);
            Scribe_Values.Look(ref PCChance, "PCChance", 0.1f);
            base.ExposeData();
        }
    }

    public class AscensionMod : Mod
    {
        AscensionSettings settings;

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public AscensionMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<AscensionSettings>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("AS_HumanoidCultivator".Translate(), ref settings.humanoidOnlyBool, "AS_HumanoidCultivatorDesc".Translate());
            listingStandard.CheckboxLabeled("AS_MachineCultivator".Translate(), ref settings.machineCultivatorBool, "AS_MachineCultivatorDesc".Translate());
            listingStandard.CheckboxLabeled("AS_OPGoldenPills".Translate(), ref settings.opGoldenPillsBool, "AS_OPGoldenPillsDesc".Translate());
            listingStandard.CheckboxLabeled("AS_LogHeals".Translate(), ref settings.logHealsBool, "AS_LogHealsDesc".Translate());
            listingStandard.Label("AS_CultivatorChance".Translate() + Math.Floor(this.settings.CultivatorChance * 1000) / 10 + "%" + "\n"+ "AS_CultivatorChanceDesc".Translate());
            settings.CultivatorChance = listingStandard.Slider(settings.CultivatorChance, 0, 1f);
            listingStandard.Label("AS_ERChance".Translate() + Math.Floor(this.settings.EssenceChance * 1000) / 10 + "%" + "\n" + "AS_ERChanceDesc".Translate());
            settings.EssenceChance = listingStandard.Slider(settings.EssenceChance, 0, 1f);
            listingStandard.Label("AS_PIChance".Translate() + Math.Floor(this.settings.PIChance * 1000) / 10 + "%" + "\n"+ "AS_PIChanceDesc".Translate());
            settings.PIChance = listingStandard.Slider(settings.PIChance, 0, 1f);
            listingStandard.Label("AS_AbilityChance".Translate() + Math.Floor(this.settings.AbilityChance * 1000) / 10 + "%" + "\n" + "AS_AbilityChanceDesc".Translate());
            settings.AbilityChance = listingStandard.Slider(settings.AbilityChance, 0, 1f);
            listingStandard.Label("AS_PCChance".Translate() + Math.Floor(this.settings.PCChance * 1000) / 10 + "%" + "\n" + "AS_PCChanceDesc".Translate());
            settings.PCChance = listingStandard.Slider(settings.PCChance, 0, 1f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "AS_Name".Translate();
        }
    }
}