using ProjectM;
using Silkworm.API;
using Silkworm.Utils;
using Stunlock.Localization;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Silkworm.Core.KeyBinding;

#nullable enable
public class Keybinding
{
    public string Id { get => Data.Id; internal set => Data.Id = value; }
    public string Name { get => Data.Name; internal set => Data.Name = value; }
    public KeyCode Primary { get => Data.Primary; internal set => Data.Primary = value; }
    public KeyCode Secondary { get => Data.Secondary; internal set => Data.Secondary = value; }
    public bool IsPressed { get => Input.GetKey(Primary) || Input.GetKey(Secondary); }
    public bool IsDown { get => Input.GetKeyDown(Primary) || Input.GetKeyDown(Secondary); }
    public bool IsUp { get => Input.GetKeyUp(Primary) || Input.GetKeyUp(Secondary); }
    public UnityEvent OnKeyPressed { get; internal set; } = new();
    public UnityEvent OnKeyDown { get; internal set; } = new();
    public UnityEvent OnKeyUp { get; internal set; } = new();

    internal KeybindingData Data;
    internal LocalizationKey NameKey;
    internal InputFlag InputFlag;
    internal KeyCode DefaultPrimary { get; set; }
    internal KeyCode DefaultSecondary { get; set; }

    public Keybinding(string id, string name, KeyCode defaultPrimary, KeyCode defaultSecondary)
    {
        Data = new KeybindingData(id, name, defaultPrimary, defaultSecondary);
        NameKey = LocalizationManager.CreateKey(name);
        DefaultPrimary = defaultPrimary;
        DefaultSecondary = defaultSecondary;

        var flag = HashUtils.Hash64(id);
        do
        {
            InputFlag = (InputFlag)flag;
        } while (Enum.IsDefined(typeof(InputFlag), (InputFlag)flag--));
    }

    public Keybinding(string id, string name, KeyCode defaultPrimary) : this(id, name, defaultPrimary, KeyCode.None) { }

    public Keybinding(string id, string name) : this(id, name, KeyCode.None) { }

    public void AddKeyPressedListener(Action action) => OnKeyPressed.AddListener(action);

    public void AddKeyDownListener(Action action) => OnKeyDown.AddListener(action);

    public void AddKeyUpListener(Action action) => OnKeyUp.AddListener(action);
}
