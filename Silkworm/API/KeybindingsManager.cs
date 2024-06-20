using ProjectM;
using Silkworm.Core.KeyBinding;
using Silkworm.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Silkworm.API;

/// <summary>
/// Primary and secondary keybinds use InputControlPaths, see Unity docs for more info:
/// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/api/UnityEngine.InputSystem.InputControlPath.html
/// </summary>
public static class KeybindingsManager
{
    internal static Dictionary<string, KeybindingCategory> Categories = new();
    internal static string ActionsFilename = "actions.json";

    public static KeybindingCategory AddCategory(string name)
    {
        if (!Categories.ContainsKey(name))
        {
            var category = new KeybindingCategory(name);
            Categories.Add(name, category);
        }

        return Categories[name];
    }

    public static Keybinding AddKeybinding(string category, string name, string defaultPrimary = null, string defaultSecondary = null)
    {
        return AddCategory(category).AddKeyBinding(name, defaultPrimary, defaultSecondary);
    }

    public static Keybinding GetKeybinding(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasKeybinding(id))
            {
                return category.GetKeybinding(id);
            }
        }
        return default;
    }

    public static Keybinding GetKeybinding(ButtonInputAction flag)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasKeybinding(flag))
            {
                return category.GetKeybinding(flag);
            }
        }
        return default;
    }

    public static void Save()
    {
        FileUtils.WriteJson(ActionsFilename, Categories);
    }

    public static void FullSave()
    {
        List<string> removeCategories = new();
        foreach (var category in Categories.Values)
        {
            category.Overrides.Clear();
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                category.Overrides.Add(keybinding.Name, keybinding.GetData());
            }

            if (category.Overrides.Count == 0)
            {
                removeCategories.Add(category.Name);
            }
        }

        foreach (var name in removeCategories)
        {
            Categories.Remove(name);
        }

        Save();
    }

    internal static void Load()
    {
        if (!FileUtils.Exists(ActionsFilename))
        {
            Save();
            return;
        }

        var categories = FileUtils.ReadJson<Dictionary<string, KeybindingCategory>>(ActionsFilename);

        if (categories != null)
        {
            Categories = categories;
        }
    }
}
