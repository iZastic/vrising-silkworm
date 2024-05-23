using Silkworm.API;
using Stunlock.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silkworm.Core.Options;

public class OptionCategory
{
    public string Name { get; internal set; }
    public Dictionary<string, bool> Toggles = new();
    public Dictionary<string, float> Sliders = new();
    public Dictionary<string, string> Dropdowns = new();

    internal readonly LocalizationKey LocalizationKey;

    internal List<string> Options = new();
    internal Dictionary<string, ToggleOption> ToggleOptions = new();
    internal Dictionary<string, SliderOption> SliderOptions = new();
    internal Dictionary<string, DropdownOption> DropdownOptions = new();
    internal Dictionary<string, string> Dividers = new();

    public OptionCategory(string name)
    {
        Name = name;
        LocalizationKey = LocalizationManager.CreateKey(name);
    }

    public ToggleOption AddToggle(string name, string description, bool defaultValue)
    {
        var option = new ToggleOption(name, description, defaultValue);
        if (Toggles.ContainsKey(name))
            option.Value = Toggles[name];

        ToggleOptions.Add(name, option);
        Options.Add(option.Name);

        return option;
    }

    public SliderOption AddSlider(string name, string description, float minValue, float maxValue, float defaultValue, int decimals = default, float stepValue = default)
    {
        var option = new SliderOption(name, description, minValue, maxValue, defaultValue, decimals);
        if (Sliders.ContainsKey(name))
            option.Value = Mathf.Clamp(Sliders[name], minValue, maxValue);

        SliderOptions.Add(name, option);
        Options.Add(option.Name);

        return option;
    }

    public DropdownOption AddDropdown(string name, string description, int defaultValue, string[] values)
    {
        var option = new DropdownOption(name, description, defaultValue, values);
        if (Dropdowns.ContainsKey(name))
            option.Value = Mathf.Max(0, Array.IndexOf(values, Dropdowns[name]));


        DropdownOptions.Add(name, option);
        Options.Add(option.Name);

        return option;
    }

    public void AddDivider()
    {
        AddDivider(null);
    }

    public void AddDivider(string name)
    {
        string id = Guid.NewGuid().ToString();
        Dividers.Add(id, name);
        Options.Add(id);
    }

    public ToggleOption GetToggle(string id)
    {
        return ToggleOptions.GetValueOrDefault(id);
    }

    public SliderOption GetSlider(string id)
    {
        return SliderOptions.GetValueOrDefault(id);
    }

    public DropdownOption GetDropdown(string id)
    {
        return DropdownOptions.GetValueOrDefault(id);
    }

    public bool HasToggle(string id)
    {
        return ToggleOptions.ContainsKey(id);
    }

    public bool HasSlider(string id)
    {
        return SliderOptions.ContainsKey(id);
    }

    public bool HasDropdown(string id)
    {
        return DropdownOptions.ContainsKey(id);
    }

    public bool TryGetToggle(string id, out ToggleOption option)
    {
        if (!ToggleOptions.ContainsKey(id))
        {
            option = null;
            return false;
        }

        option = ToggleOptions[id];
        return true;
    }

    public bool TryGetSlider(string id, out SliderOption option)
    {
        if (!SliderOptions.ContainsKey(id))
        {
            option = null;
            return false;
        }

        option = SliderOptions[id];
        return true;
    }

    public bool TryGetDropdown(string id, out DropdownOption option)
    {
        if (!DropdownOptions.ContainsKey(id))
        {
            option = null;
            return false;
        }

        option = DropdownOptions[id];
        return true;
    }

    public bool TryGetDivider(string id, out string text)
    {
        if (!Dividers.ContainsKey(id))
        {
            text = null;
            return false;
        }

        text = Dividers[id];
        return true;
    }
}
