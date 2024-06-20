using HarmonyLib;
using Il2CppSystem;
using ProjectM;
using Silkworm.API;
using Silkworm.Utils;
using Stunlock.Localization;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static ProjectM.InputActionSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

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

        // TODO: Load __instance.._ControlsVisualMapping.ButtonInputActionData
        // TODO: Load __instance._ControlsVisualMapping.AnalogInputActionData
    }

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.TryGetButtonInputActionLocalization))]
    //private static bool TryGetButtonInputActionLocalization(ref bool __result, ButtonInputAction buttonInput, ref LocalizationKey name, ref LocalizationKey desc)
    //{
    //    Plugin.Logger.LogInfo(buttonInput);
    //    var keybinding = KeybindingsManager.GetKeybinding(buttonInput);
    //    if (keybinding == null)
    //    {
    //        return true;
    //    }

    //    Plugin.Logger.LogInfo(keybinding.Name);
    //    name = keybinding.NameKey;
    //    desc = keybinding.NameKey;
    //    __result = true;
    //    return false;
    //}

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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.ModifyInputActionBinding), typeof(ButtonInputAction), typeof(bool), typeof(Action<bool>), typeof(Action<bool, bool>), typeof(OnRebindCollision), typeof(Nullable_Unboxed<ControllerType>))]
    private static void ModifyKeyInputSetting(ButtonInputAction buttonInput, bool modifyPrimary, ref Action<bool> onComplete, ref Action<bool, bool> onCancel, OnRebindCollision onCollision, Nullable_Unboxed<ControllerType> overrideControllerType)
    {
        Plugin.Logger.LogInfo("ModifyKeyInputSetting Primary" +
            "\n\tButtonInput: " + buttonInput +
            "\n\tPrimary: " + modifyPrimary +
            "\n\tOnCollision: " + onCollision +
            "\n\tOverrideControllerType: " + overrideControllerType
        );
        /*
         * TODO: Why is this canceling immediately without a popup
         */
        onComplete += (Action<bool>)(b1 =>
        {
            Plugin.Logger.LogInfo("onComplete " + b1);
        });
        onCancel += (Action<bool, bool>)((b1, b2) =>
        {
            Plugin.Logger.LogInfo("onCancel " + b1 + ", " + b2);
        });

        //var keybinding = KeybindingsManager.GetKeybinding(buttonInput);

        //if (keybinding == null)
        //{
        //    return;
        //}

        //keybinding.InputAction
        //    .PerformInteractiveRebinding()
        //    .WithTargetBinding(0)
        //    .OnComplete((Action<RebindingOperation>)(operation =>
        //    {

        //    }))
        //    .OnCancel((Action<RebindingOperation>)(operation =>
        //    {

        //    }))
        //    .Start();

        //if (primary)
        //    keybinding.Primary = newKey;
        //else
        //    keybinding.Secondary = newKey;

        //KeybindingsManager.FullSave();

        //return false;
    }

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
