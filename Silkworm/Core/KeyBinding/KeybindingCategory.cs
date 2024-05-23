using ProjectM;
using Silkworm.API;
using Stunlock.Localization;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Silkworm.Core.KeyBinding;

public class KeybindingCategory
{
    public string Name { get; internal set; }
    public Dictionary<string, Keybinding.Data> Overrides = new();

    internal LocalizationKey NameKey;
    internal InputActionMap InputActionMap;

    internal Dictionary<string, Keybinding> KeybindingMap = new();
    internal Dictionary<ButtonInputAction, string> KeybindingFlags = new();

    public KeybindingCategory(string name)
    {
        Name = name;
        InputActionMap = new InputActionMap(Name);
        NameKey = LocalizationManager.CreateKey(Name);
    }

    public Keybinding AddKeyBinding(string name, string defaultPrimary = null, string defaultSecondary = null)
    {
        var keybinding = new Keybinding(InputActionMap.AddAction(name), defaultPrimary, defaultSecondary);
        if (Overrides.TryGetValue(name, out var data))
        {
            if (!string.IsNullOrEmpty(data.PrimaryOverride)) keybinding.Override(true, data.PrimaryOverride);
            if (!string.IsNullOrEmpty(data.SecondaryOverride)) keybinding.Override(false, data.SecondaryOverride);
        }
        KeybindingMap.Add(name, keybinding);
        KeybindingFlags.Add(keybinding.InputFlag, name);
        return keybinding;
    }

    public Keybinding GetKeybinding(string id)
    {
        return KeybindingMap.GetValueOrDefault(id);
    }

    public Keybinding GetKeybinding(ButtonInputAction flag)
    {
        var id = KeybindingFlags.GetValueOrDefault(flag);
        return id == null ? default : GetKeybinding(id);
    }

    public bool HasKeybinding(string id)
    {
        return KeybindingMap.ContainsKey(id);
    }

    public bool HasKeybinding(ButtonInputAction flag)
    {
        var id = KeybindingFlags.GetValueOrDefault(flag);
        return id != null && HasKeybinding(id);
    }
}
