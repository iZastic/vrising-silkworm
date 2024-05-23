using Silkworm.API;
using System;
using UnityEngine;

namespace SilkwormTest
{
    public class SilkwormTest : MonoBehaviour
    {
        public enum DropdownValues
        {
            None,
            One,
            Two,
            Three,
            Four,
            Five
        }

        private void Awake()
        {
            SetupOptions();
            SetupKeybinds();
        }

        private void SetupOptions()
        {
            var category = OptionsManager.AddCategory("SilkwormTest");
            category.AddDropdown("Dropdown", "Silkworm test dropdown", 0, Enum.GetNames<DropdownValues>()).AddListener(value =>
            {
                Plugin.Logger.LogInfo("Dropdown: " + Enum.GetName((DropdownValues)value));
            });
            category.AddDivider();
            category.AddSlider("Slider 1", "Silkworm test slider 1", 0, 10, 5, 0).AddListener(value =>
            {
                Plugin.Logger.LogInfo("Slider 1: " + value.ToString("0.00"));
            });
            category.AddSlider("Slider 2", "Silkworm test slider 2", 0, 1, 0.5f, 1, 0.1f).AddListener(value =>
            {
                Plugin.Logger.LogInfo("Slider 2: " + value.ToString("0.00"));
            });
            category.AddToggle("Toggle", "Silkworm test toggle", false).AddListener(value =>
            {
                Plugin.Logger.LogInfo("Toggle: " + value);
            });
        }

        private void SetupKeybinds()
        {
            var category = KeybindingsManager.AddCategory("SilkwormTest");
            var keybind1 = category.AddKeyBinding("Keybind 1", "/Mouse/forwardButton");
            // Run once with this uncommented and save to generate overrides.json, then comment out to load saved override
            //keybind1.Override(true, "/Mouse/backButton");
            keybind1.AddKeyDownListener(() => Plugin.Logger.LogInfo("Keybind 1 -> Down"));
            keybind1.AddKeyPressedListener(() => Plugin.Logger.LogInfo("Keybind 1 -> Pressed"));
            keybind1.AddKeyUpListener(() => Plugin.Logger.LogInfo("Keybind 1 -> Up"));

            var keybind2 = category.AddKeyBinding("Keybind 2", "/Keyboard/space");
            keybind2.AddKeyDownListener(() => Plugin.Logger.LogInfo("Keybind 2 -> Down"));
            keybind2.AddKeyPressedListener(() => Plugin.Logger.LogInfo("Keybind 2 -> Pressed"));
            keybind2.AddKeyUpListener(() => Plugin.Logger.LogInfo("Keybind 2 -> Up"));

            var keybind3 = category.AddKeyBinding("Save Keybinds", "/Keyboard/F10");
            keybind3.AddKeyUpListener(() =>
            {
                Plugin.Logger.LogInfo("Save Keybinds");
                KeybindingsManager.FullSave();
            });
        }
    }
}
