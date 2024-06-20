using ProjectM;
using Silkworm.API;
using Silkworm.Utils;
using Stunlock.Localization;
using System;
using UnityEngine.InputSystem;

namespace Silkworm.Core.KeyBinding;

public delegate void KeyEvent();

public class Keybinding
{
    public struct Data
    {
        public string Name;
        public string PrimaryDefault;
        public string PrimaryOverride;
        public string SecondaryDefault;
        public string SecondaryOverride;
    }

    public string Name { get => InputAction.name; internal set => InputAction.Rename(value); }
    public string Primary => InputAction.bindings[0].effectivePath;
    public string Secondary => InputAction.bindings[1].effectivePath;
    public bool IsPressed => InputAction.IsPressed();
    public bool IsDown => InputAction.WasPressedThisFrame();
    public bool IsUp => InputAction.WasReleasedThisFrame();

    internal event KeyEvent KeyPressed = delegate { };
    internal event KeyEvent KeyDown = delegate { };
    internal event KeyEvent KeyUp = delegate { };

    internal ButtonInputAction InputFlag;
    internal LocalizationKey NameKey;
    internal InputAction InputAction;
    internal string DefaultPrimary { get; set; }
    internal string DefaultSecondary { get; set; }
    internal string PrimaryName => InputAction.bindings[0].ToDisplayString();
    internal string SecondaryName => InputAction.bindings[1].ToDisplayString();

    internal Keybinding(InputAction inputAction, string defaultPrimary = null, string defaultSecondary = null)
    {
        InputAction = inputAction;
        NameKey = LocalizationManager.CreateKey(InputAction.name);

        InputAction.AddBinding(defaultPrimary == null ? "" : defaultPrimary);
        DefaultPrimary = defaultPrimary == null ? "" : defaultPrimary;

        InputAction.AddBinding(defaultSecondary == null ? "" : defaultSecondary);
        DefaultSecondary = defaultSecondary == null ? "" : defaultSecondary;

        var flag = HashUtils.Hash64(InputAction.actionMap.name + "." + InputAction.name);
        do
        {
            InputFlag = (ButtonInputAction)flag;
        } while (Enum.IsDefined(typeof(ButtonInputAction), (ButtonInputAction)flag--));
    }

    /// <summary>
    /// Is called each frame the key is held down
    /// </summary>
    /// <param name="action"></param>
    public void AddKeyPressedListener(KeyEvent action) => KeyPressed += action;

    /// <summary>
    /// Is called during the frame the key is pressed
    /// </summary>
    /// <param name="action"></param>
    public void AddKeyDownListener(KeyEvent action) => KeyDown += action;

    /// <summary>
    /// Is called during the frame the key is released
    /// </summary>
    /// <param name="action"></param>
    public void AddKeyUpListener(KeyEvent action) => KeyUp += action;

    public void Override(bool primary, string path)
    {
        InputAction.ApplyBindingOverride(primary ? 0 : 1, path);
    }

    internal void OnKeyPressed() => KeyPressed();

    internal void OnKeyDown() => KeyDown();

    internal void OnKeyUp() => KeyUp();

    internal Data GetData()
    {
        return new Data
        {
            Name = Name,
            PrimaryDefault = DefaultPrimary,
            PrimaryOverride = InputAction.bindings[0].hasOverrides ? Primary : null,
            SecondaryDefault = DefaultSecondary,
            SecondaryOverride = InputAction.bindings[1].hasOverrides ? Secondary : null,
        };
    }
}
