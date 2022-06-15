using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using ProjectM.UI;
using Silkworm.API;
using Silkworm.Core.Options;
using StunShared.UI;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal static class OptionsPanel_Interface_Hook
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(OptionsPanel_Interface), nameof(OptionsPanel_Interface.Start))]
    private static void Start(OptionsPanel_Interface __instance)
    {
        foreach (var category in OptionsManager.Categories.Values)
        {
            var header = UIHelper.InstantiatePrefabUnderAnchor(__instance.CategoryHeaderPrefab, __instance.ContentNode);
            header.SetText(category.LocalizationKey);

            foreach (var id in category.Options)
            {
                if (category.TryGetToggle(id, out var toggleOption))
                {
                    var checkbox = UIHelper.InstantiatePrefabUnderAnchor(__instance.CheckboxPrefab, __instance.ContentNode);
                    checkbox.Initialize(
                        toggleOption.NameKey,
                        toggleOption.Value,
                        OnChange(toggleOption)
                    );
                    toggleOption.AddListener(value => checkbox.Toggle.isOn = value);

                }
                else if (category.TryGetSlider(id, out var sliderOption))
                {
                    var slider = UIHelper.InstantiatePrefabUnderAnchor(__instance.SliderPrefab, __instance.ContentNode);
                    slider.Initialize(
                        sliderOption.NameKey,
                        LocalizationManager.GetFormatKey(sliderOption.ValueFormat),
                        sliderOption.MinValue,
                        sliderOption.MaxValue,
                        sliderOption.Value,
                        OnChange(sliderOption),
                        (Il2CppSystem.Func<float, float>)(value =>
                        {
                            if (sliderOption.Value < 0)
                            {
                                return Mathf.Round(sliderOption.Value * 100f) / 100f;
                            }
                            return Mathf.Round(sliderOption.Value);
                        })
                    );
                    sliderOption.AddListener(value => slider.Slider.value = value);
                }
                else if (category.TryGetDropdown(id, out var dropdownOption))
                {
                    var dropdown = UIHelper.InstantiatePrefabUnderAnchor(__instance.DropdownPrefab, __instance.ContentNode);
                    var values = new List<string>();
                    dropdownOption.Values.ToList().ForEach(x => values.Add(x));
                    dropdown.Initialize(
                        dropdownOption.NameKey,
                        values,
                        dropdownOption.Value,
                        OnChange(dropdownOption)
                    );
                    dropdownOption.AddListener(value => dropdown.Dropdown.value = value);
                }
            }
        }
    }

    private static UnityAction<T> OnChange<T>(Option<T> option)
    {
        return (UnityAction<T>)(value =>
        {
            option.OnChange.Invoke(value);
            OptionsManager.Save();
        });
    }
}
