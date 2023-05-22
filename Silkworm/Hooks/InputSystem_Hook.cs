using HarmonyLib;
using ProjectM;
using Silkworm.API;
using Stunlock.Localization;
using System.Linq;
using UnityEngine;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal static class InputSystem_Hook
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputSystem), nameof(InputSystem.GetKeyInputMap))]
    private static bool GetKeyInputMap(InputSystem __instance, InputFlag input, ref string inputText, ref Sprite inputIcon, bool primary)
    {
        var keybinding = KeybindingsManager.GetKeybinding(input);

        if (keybinding == null)
            return true;

        var keyCode = primary ? keybinding.Primary : keybinding.Secondary;
        if (keyCode == KeyCode.None)
            return false;

        var keysData = __instance._ControlsVisualMapping.KeysData.FirstOrDefault(x => x.KeyCode == keyCode);
        inputText = keysData == null ? __instance.GetKeyCodeString(keyCode) : Localization.Get(keysData.TextKey);
        if (keysData != null)
            inputIcon = keysData.KeySprite;

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputSystem), nameof(InputSystem.ModifyKeyInputSetting))]
    private static bool ModifyKeyInputSetting(InputFlag inputFlag, KeyCode newKey, bool primary)
    {
        var keybinding = KeybindingsManager.GetKeybinding(inputFlag);

        if (keybinding == null)
            return true;

        if (primary)
            keybinding.Primary = newKey;
        else
            keybinding.Secondary = newKey;

        KeybindingsManager.FullSave();

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputSystem), nameof(InputSystem.OnUpdate))]
    private static void OnUpdate()
    {
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                if (keybinding.IsDown)
                    keybinding.OnKeyDown?.Invoke();
                if (keybinding.IsPressed)
                    keybinding.OnKeyPressed?.Invoke();
                if (keybinding.IsUp)
                    keybinding.OnKeyUp?.Invoke();
            }
        }
    }
}
