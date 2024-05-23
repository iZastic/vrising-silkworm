using HarmonyLib;
using Il2CppSystem;
using ProjectM;
using Silkworm.API;
using Silkworm.Utils;
using Stunlock.Localization;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal static class InputActionSystem_Hook
{
    private static InputActionMap MC_InputActionMap;
    private static InputAction ActionModeInputAction;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.OnCreate))]
    private static void OnCreate(InputActionSystem __instance)
    {
        __instance._LoadedInputActions.Disable();
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            __instance._LoadedInputActions.AddActionMap(category.InputActionMap);
        }
        __instance._LoadedInputActions.Enable();
    }

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.GetBindingDisplayInfo))]
    //private static bool GetKeyInputMap(InputActionSystem __instance, BindingDisplayInfo __result, ButtonInputAction buttonInput, bool primary)
    //{
    //    var keybinding = KeybindingsManager.GetKeybinding(buttonInput);

    //    if (keybinding == null)
    //        return true;

    //    var keyCode = primary ? keybinding.Primary : keybinding.Secondary;
    //    if (keyCode == KeyCode.None)
    //        return false;

    //    var keysData = __instance._ControlsVisualMapping.KeysData.FirstOrDefault(x => x.KeyCode == keyCode);
    //    inputText = keysData == null ? __instance.GetKeyCodeString(keyCode) : Localization.Get(keysData.TextKey);
    //    if (keysData != null)
    //        inputIcon = keysData.KeySprite;

    //    return false;
    //}

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.InputAction))]
    //private static bool ModifyKeyInputSetting(ButtonInputAction buttonInput, bool modifyPrimary, Action<bool> onComplete, Action<bool, bool> onCancel)
    //{
    //    var keybinding = KeybindingsManager.GetKeybinding(buttonInput);

    //    if (keybinding == null)
    //        return true;

    //    if (primary)
    //        keybinding.Primary = newKey;
    //    else
    //        keybinding.Secondary = newKey;

    //    KeybindingsManager.FullSave();

    //    return false;
    //}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.OnUpdate))]
    private static void OnUpdate()
    {
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                if (keybinding.IsDown)
                    keybinding.OnKeyDown();
                if (keybinding.IsPressed)
                    keybinding.OnKeyPressed();
                if (keybinding.IsUp)
                    keybinding.OnKeyUp();
            }
        }
    }
}
