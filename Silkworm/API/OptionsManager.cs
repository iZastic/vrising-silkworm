using System.Collections.Generic;
using Silkworm.Utils;
using Silkworm.Core.Options;

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

    public static ToggleOption AddToggle(string category, string id, string text, bool defaultValue)
    {
        return AddCategory(category).AddToggle(id, text, defaultValue);
    }

    public static SliderOption AddSlider(string category, string id, string text, float minValue, float maxValue, float defaultValue)
    {
        return AddSlider(category, id, text, minValue, maxValue, defaultValue, LocalizationManager.Format.Default);
    }

    public static SliderOption AddSlider(string category, string id, string text, float minValue, float maxValue, float defaultValue, string format)
    {
        return AddCategory(category).AddSlider(id, text, minValue, maxValue, defaultValue, format);
    }

    public static DropdownOption AddDropdown(string category, string id, string name, int defaultValue, string[] values)
    {
        return AddCategory(category).AddDropdown(id, name, defaultValue, values);
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
                category.Toggles.Add(toggle.Id, toggle.Value);
            foreach (var slider in category.SliderOptions.Values)
                category.Sliders.Add(slider.Id, slider.Value);
            foreach (var dropdown in category.DropdownOptions.Values)
                category.Dropdowns.Add(dropdown.Id, dropdown.Values[dropdown.Value]);

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
            Save();

        var categories = FileUtils.ReadJson<Dictionary<string, OptionCategory>>(OptionsFilename);
        if (categories != null)
            Categories = categories;
    }
}
