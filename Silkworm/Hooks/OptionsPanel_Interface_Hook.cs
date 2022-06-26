using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using ProjectM.UI;
using Silkworm.API;
using Silkworm.Core.Options;
using StunShared.UI;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using TMPro;

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
                            if (sliderOption.Value < 10)
                                return Mathf.Round(sliderOption.Value * 10f) / 10f;
                            else if (sliderOption.Value < 0)
                                return Mathf.Round(sliderOption.Value * 100f) / 100f;
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
                else if (category.TryGetDivider(id, out var dividerText))
                {
                    var divider = CreateDivider(__instance.ContentNode, dividerText);
                }
            }
        }
    }

    private static GameObject CreateDivider(Transform parent, string text)
    {
        var textComps = parent.GetComponentsInChildren<TextMeshProUGUI>();
        var divider = new GameObject("Divider");

        var dividerTransform = divider.AddComponent<RectTransform>();
        dividerTransform.SetParent(parent);
        dividerTransform.localScale = Vector3.one;
        dividerTransform.sizeDelta = new Vector2(0, 28);

        var dividerImage = divider.AddComponent<Image>();
        dividerImage.color = new Color(0.12f, 0.152f, 0.2f, 0.15f);

        var dividerLayout = divider.AddComponent<LayoutElement>();
        dividerLayout.preferredHeight = 28;

        var dividerTextObject = new GameObject("Text");
        var dividerTextTransform = dividerTextObject.AddComponent<RectTransform>();
        dividerTextTransform.SetParent(divider.transform);
        dividerTextTransform.localScale = Vector3.one;

        var dividerText = dividerTextObject.AddComponent<TextMeshProUGUI>();
        dividerText.alignment = TextAlignmentOptions.Center;
        dividerText.fontStyle = FontStyles.SmallCaps;
        dividerText.font = textComps[0].font;
        dividerText.fontSize = 20;
        if (text != null)
            dividerText.SetText(text);

        return divider;
    }

    private static UnityAction<T> OnChange<T>(Option<T> option)
    {
        return (UnityAction<T>)(value =>
        {
            option.SetValue(value);
            OptionsManager.FullSave();
        });
    }
}
