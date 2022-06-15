using BepInEx.IL2CPP.Hook;
using HarmonyLib;
using ProjectM;
using Silkworm.API;
using Silkworm.Utils;
using StunLocalization;
using System;
using System.Linq;
using UnityEngine;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal static class InputSystem_Hook
{
    private unsafe delegate bool TryGetInputFlagLocalization(IntPtr _this, InputFlag inputFlag, LocalizationKey* locKey);
    private static TryGetInputFlagLocalization TryGetInputFlagLocalization_Original;
    private static FastNativeDetour TryGetInputFlagLocalization_Detour;

    internal static void CreateAndApply()
    {
        unsafe
        {
            TryGetInputFlagLocalization_Detour = DetourUtils.Create(typeof(InputSystem), "TryGetInputFlagLocalization", TryGetInputFlagLocalization_Hook, out TryGetInputFlagLocalization_Original);
        }
    }

    internal static void Dispose()
    {
        if (TryGetInputFlagLocalization_Detour != null)
            TryGetInputFlagLocalization_Detour.Dispose();
    }

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

        KeybindingsManager.Save();

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

    private static unsafe bool TryGetInputFlagLocalization_Hook(IntPtr _this, InputFlag inputFlag, LocalizationKey* locKey)
    {
        var keybinding = KeybindingsManager.GetKeybinding(inputFlag);

        if (keybinding != null)
        {
            *locKey = keybinding.NameKey;
            return true;
        }

        return TryGetInputFlagLocalization_Original(_this, inputFlag, locKey);
    }
}
