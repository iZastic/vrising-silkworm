using HarmonyLib;
using ProjectM;
using Silkworm.API;
using Stunlock.Localization;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal static class Localization_Hook
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Localization), nameof(Localization.Get), typeof(AssetGuid), typeof(bool))]
    private static bool Get(AssetGuid guid, ref string __result)
    {
        if (!LocalizationManager.HasKey(guid))
            return true;

        __result = LocalizationManager.GetKey(guid);
        return false;
    }
}
