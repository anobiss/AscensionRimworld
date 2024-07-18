using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine; // Make sure to include this for Event.current
using System.Collections.Generic;

namespace Ascension
{
    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    public static class Ascension_PlaySettings_DoPlaySettingsGlobalControls_Patch
    {
        public static void Postfix(WidgetRow row, bool worldView)
        {
            if (!worldView)
            {
                // First toggleable icon
                row.ToggleableIcon(
                    tooltip: string.Format("{0}: {1}\n\n{2}",
                                           "HotKeyTip".Translate(),
                                           KeyPrefs.KeyPrefsData.GetBoundKeyCode(AscensionDefOf.ToggleQiDisplay, KeyPrefs.BindingSlot.A).ToStringReadable(),
                                           "AS_ShowQiToggleButton".Translate()),
                    toggleable: ref LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().displayQiGrid,
                    tex: AscensionTextures.ToggleQi,
                    mouseoverSound: SoundDefOf.Mouseover_ButtonToggle
                );

                // Check for first keybinding toggle
                CheckKeyBindingToggle(AscensionDefOf.ToggleQiDisplay, ref LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().displayElementGrid);

                // Second toggleable icon
                row.ToggleableIcon(
                    tooltip: string.Format("{0}: {1}\n\n{2}",
                                           "HotKeyTip".Translate(),
                                           KeyPrefs.KeyPrefsData.GetBoundKeyCode(AscensionDefOf.ToggleElementDisplay, KeyPrefs.BindingSlot.A).ToStringReadable(),
                                           "AS_ShowElementToggleButton".Translate()),
                    toggleable: ref LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().displayElementGrid,
                    tex: AscensionTextures.ToggleElement, // Use a different texture if available
                    mouseoverSound: SoundDefOf.Mouseover_ButtonToggle
                );

                // Check for second keybinding toggle
                CheckKeyBindingToggle(AscensionDefOf.ToggleElementDisplay, ref LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().displayElementGrid);
            }
        }
        // Method to check the keybinding and toggle the state
        private static void CheckKeyBindingToggle(KeyBindingDef keyBindingDef, ref bool toggle)
        {
            if (keyBindingDef.KeyDownEvent)
            {
                toggle = !toggle;
                Event.current.Use();
            }
        }
    }
}
