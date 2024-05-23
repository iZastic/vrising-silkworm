using System.Collections.Generic;
using Silkworm.Utils;
using Silkworm.Core.Options;
using System;

namespace Silkworm.API;

public static class OptionsManager
{
    internal static Dictionary<string, OptionCategory> Categories = new();
    internal static string OptionsFilename = "options.json";

    public static OptionCategory AddCategory(string name)
    {
        if (!Categories.ContainsKey(name))
            Categories.Add(name, new OptionCategory(name));

        return Categories[name];
    }

    public static ToggleOption GetToggle(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasToggle(id))
                return category.GetToggle(id);
        }
        return default;
    }

    public static SliderOption GetSlider(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasSlider(id))
                return category.GetSlider(id);
        }
        return default;
    }

    public static DropdownOption GetDropdown(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasDropdown(id))
                return category.GetDropdown(id);
        }
        return default;
    }

    public static void Save()
    {
        FileUtils.WriteJson(OptionsFilename, Categories);
    }

    internal static void FullSave()
    {
        List<string> removeCategories = new();
        foreach (var category in Categories.Values)
        {
            category.Toggles.Clear();
            category.Sliders.Clear();
            category.Dropdowns.Clear();

            foreach (var toggle in category.ToggleOptions.Values)
                category.Toggles.Add(toggle.Name, toggle.Value);
            foreach (var slider in category.SliderOptions.Values)
                category.Sliders.Add(slider.Name, slider.Value);
            foreach (var dropdown in category.DropdownOptions.Values)
                category.Dropdowns.Add(dropdown.Name, dropdown.Values[dropdown.Value]);

            if (category.Toggles.Count + category.Sliders.Count + category.Dropdowns.Count == 0)
                removeCategories.Add(category.Name);
        }

        foreach (var name in removeCategories)
            Categories.Remove(name);

        Save();
    }

    internal static void Load()
    {
        if (!FileUtils.Exists(OptionsFilename))
        {
            Save();
            return;
        }

        var categories = FileUtils.ReadJson<Dictionary<string, OptionCategory>>(OptionsFilename);
        if (categories != null)
            Categories = categories;
    }
}
