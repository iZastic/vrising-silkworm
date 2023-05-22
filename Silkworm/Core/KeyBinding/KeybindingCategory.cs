using ProjectM;
using Silkworm.API;
using Stunlock.Localization;
using System.Collections.Generic;
using UnityEngine;

namespace Silkworm.Core.KeyBinding;

public class KeybindingCategory
{
    public string Name { get; internal set; }
    public Dictionary<string, KeybindingData> Keybindings = new();

    internal LocalizationKey NameKey;

    internal Dictionary<string, Keybinding> KeybindingMap = new();
    internal Dictionary<InputFlag, string> KeybindingFlags = new();

    public KeybindingCategory(string name)
    {
        Name = name;
        NameKey = LocalizationManager.CreateKey(name);
    }

    public Keybinding AddKeyBinding(string id, string name, KeyCode defaultPrimary, KeyCode defaultSecondary)
    {
        var keybinding = new Keybinding(id, name, defaultPrimary, defaultSecondary);
        if (Keybindings.ContainsKey(id))
        {
            keybinding.Primary = Keybindings[id].Primary;
            keybinding.Secondary = Keybindings[id].Secondary;
        }
        KeybindingMap.Add(id, keybinding);
        KeybindingFlags.Add(keybinding.InputFlag, id);
        return keybinding;
    }

    public Keybinding AddKeyBinding(string id, string name, KeyCode defaultPrimary)
    {
        return AddKeyBinding(id, name, defaultPrimary, KeyCode.None);
    }

    public Keybinding AddKeyBinding(string id, string name)
    {
        return AddKeyBinding(id, name, KeyCode.None);
    }

    public Keybinding GetKeybinding(string id)
    {
        return KeybindingMap.GetValueOrDefault(id);
    }

    public Keybinding GetKeybinding(InputFlag flag)
    {
        var id = KeybindingFlags.GetValueOrDefault(flag);
        return id == null ? default : GetKeybinding(id);
    }

    public bool HasKeybinding(string id)
    {
        return KeybindingMap.ContainsKey(id);
    }

    public bool HasKeybinding(InputFlag flag)
    {
        var id = KeybindingFlags.GetValueOrDefault(flag);
        return id != null && HasKeybinding(id);
    }
}
