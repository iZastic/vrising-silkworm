using ProjectM;
using Silkworm.Core.KeyBinding;
using Silkworm.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Silkworm.API;

public static class KeybindingsManager
{
    internal static Dictionary<string, KeybindingCategory> Categories = new();
    internal static string KeybindingFilename = "keybindings.json";

    public static KeybindingCategory AddCategory(string name)
    {
        if (!Categories.ContainsKey(name))
            Categories.Add(name, new KeybindingCategory(name));

        return Categories[name];
    }

    public static Keybinding AddKeybinding(string category, string id, string name, KeyCode defaultPrimary, KeyCode defaultSecondary)
    {
        return AddCategory(category).AddKeyBinding(id, name, defaultPrimary, defaultSecondary);
    }

    public static Keybinding AddKeybinding(string category, string id, string name, KeyCode defaultPrimary)
    {
        return AddKeybinding(category, id, name, defaultPrimary, KeyCode.None);
    }

    public static Keybinding AddKeybinding(string category, string id, string name)
    {
        return AddKeybinding(category, id, name, KeyCode.None);
    }

    public static Keybinding GetKeybinding(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasKeybinding(id))
                return category.GetKeybinding(id);
        }
        return default;
    }

    public static Keybinding GetKeybinding(InputFlag flag)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasKeybinding(flag))
                return category.GetKeybinding(flag);
        }
        return default;
    }

    internal static void Save()
    {
        List<string> removeCategories = new();
        foreach (var category in Categories.Values)
        {
            category.Keybindings.Clear();
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                category.Keybindings.Add(keybinding.Id, keybinding.Data);
            }

            if (category.Keybindings.Count == 0)
                removeCategories.Add(category.Name);
        }

        foreach (var name in removeCategories)
            Categories.Remove(name);

        FileUtils.WriteJson(KeybindingFilename, Categories);
    }

    internal static void Load()
    {
        if (!FileUtils.Exists(KeybindingFilename))
            Save();

        var categories = FileUtils.ReadJson<Dictionary<string, KeybindingCategory>>(KeybindingFilename);

        if (categories != null)
            Categories = categories;
    }
}
