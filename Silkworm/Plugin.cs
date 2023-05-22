using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Silkworm.API;

namespace Silkworm;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static ManualLogSource Logger;

    private static Harmony harmony;

    public override void Load()
    {
        Logger = Log;

        harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        OptionsManager.Load();
        KeybindingsManager.Load();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} v{PluginInfo.PLUGIN_VERSION} is loaded!");
    }

    public override bool Unload()
    {
        OptionsManager.FullSave();
        KeybindingsManager.FullSave();

        return true;
    }
}
