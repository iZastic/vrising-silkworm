using HarmonyLib;
using ProjectM.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal class OptionsPanel_Graphics_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(OptionsPanel_Graphics), nameof(OptionsPanel_Graphics.Start))]
    private static void Start()
    {

    }
}
