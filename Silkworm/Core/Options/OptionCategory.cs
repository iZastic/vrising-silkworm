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

    public ToggleOption AddToggle(string id, string name, bool defaultValue)
    {
        var option = new ToggleOption(id, name, defaultValue);
        if (Toggles.ContainsKey(id))
            option.Value = Toggles[id];

        ToggleOptions.Add(id, option);
        Options.Add(option.Id);

        return option;
    }

    public SliderOption AddSlider(string id, string name, float minValue, float maxValue, float defaultValue)
    {
        return AddSlider(id, name, minValue, maxValue, defaultValue, LocalizationManager.Format.Default);
    }

    public SliderOption AddSlider(string id, string name, float minValue, float maxValue, float defaultValue, string format)
    {
        var option = new SliderOption(id, name, minValue, maxValue, defaultValue, format);
        if (Sliders.ContainsKey(id))
            option.Value = Mathf.Clamp(Sliders[id], minValue, maxValue);

        SliderOptions.Add(id, option);
        Options.Add(option.Id);

        return option;
    }

    public DropdownOption AddDropdown(string id, string name, int defaultValue, string[] values)
    {
        var option = new DropdownOption(id, name, defaultValue, values);
        if (Dropdowns.ContainsKey(id))
            option.Value = Mathf.Max(0, Array.IndexOf(values, Dropdowns[id]));


        DropdownOptions.Add(id, option);
        Options.Add(option.Id);

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
