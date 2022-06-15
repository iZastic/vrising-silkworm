using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Silkworm.API;
using Silkworm.Core.Options;
using Silkworm.Hooks;
using System;
using UnhollowerRuntimeLib;

namespace Silkworm;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static ManualLogSource Logger;

    private static Harmony harmony;

    public override void Load()
    {
        Logger = Log;

        ClassInjector.RegisterTypeInIl2Cpp<Silkworm>();
        AddComponent<Silkworm>();

        harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        OptionsManager.Load();
        KeybindingsManager.Load();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} v{PluginInfo.PLUGIN_VERSION} is loaded!");
    }

    public override bool Unload()
    {
        OptionsManager.Save();
        KeybindingsManager.Save();

        return true;
    }
}
