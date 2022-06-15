using HarmonyLib;
using ProjectM.UI;
using Silkworm.API;
using StunShared.UI;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal class Options_ControlsPanel_Hook
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Options_ControlsPanel), nameof(Options_ControlsPanel.Start))]
    private static void Start(Options_ControlsPanel __instance)
    {
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            var categoryHeader = UIHelper.InstantiatePrefabUnderAnchor(__instance.CategoryHeaderPrefab, __instance.ContentNode);
            categoryHeader.SetText(category.NameKey);

            foreach (var keybinding in category.KeybindingMap.Values)
            {
                __instance.AddEntry(keybinding.InputFlag);
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Options_ControlsPanel), nameof(Options_ControlsPanel.OnResetButtonClicked))]
    private static void OnResetButtonClicked()
    {
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                keybinding.Primary = keybinding.DefaultPrimary;
                keybinding.Secondary = keybinding.DefaultSecondary;
            }
        }

        KeybindingsManager.Save();
    }
}
