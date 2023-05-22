using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Silkworm.API;
using System;

namespace SilkwormTest
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        internal static ManualLogSource Logger;

        private enum TestEnum
        {
            Test0,
            Test1,
            Test2,
            Test3
        }

        public override void Load()
        {
            Logger = Log;

            var category1 = OptionsManager.AddCategory("Silkworm 1");
            category1.AddToggle("silkworm.testtoggle", "Test Toggle", false).AddListener(value =>
            {
                Logger.LogMessage($"Test Toggle: {value}");
            });
            category1.AddSlider("silkworm.testslider", "Test Slider", 0, 100, 50).AddListener(value =>
            {
                Logger.LogMessage($"Test Slider: {value}");
            });
            category1.AddDropdown("silkworm.testdropdown", "Test Dropdown", (int)TestEnum.Test2, Enum.GetNames(typeof(TestEnum))).AddListener(value =>
            {
                Logger.LogMessage($"Test Dropdown: {value}");
            });

            var category2 = OptionsManager.AddCategory("Silkworm 2");
            category2.AddToggle("silkworm.testtoggle", "Test Toggle", false).AddListener(value =>
            {
                Logger.LogMessage($"Test Toggle: {value}");
            });
            category2.AddSlider("silkworm.testslider", "Test Slider", 0, 100, 50).AddListener(value =>
            {
                Logger.LogMessage($"Test Slider: {value}");
            });
            category2.AddDropdown("silkworm.testdropdown", "Test Dropdown", (int)TestEnum.Test2, Enum.GetNames(typeof(TestEnum))).AddListener(value =>
            {
                Logger.LogMessage($"Test Dropdown: {value}");
            });

            var keybindingCategory = KeybindingsManager.AddCategory("Silkworm 1");
            keybindingCategory.AddKeyBinding("silkworm.testkey", "Test Key", UnityEngine.KeyCode.F5);

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
