using HarmonyLib;
using ProjectM.UI;
using Silkworm.API;
using Silkworm.Core.Options;
using StunShared.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Il2CppSystem;

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
            __instance.AddHeader(category.LocalizationKey);

            foreach (var id in category.Options)
            {
                if (category.TryGetToggle(id, out var toggleOption))
                {
                    var checkbox = UIHelper.InstantiatePrefabUnderAnchor(__instance.CheckboxPrefab, __instance.ContentNode);
                    checkbox.Initialize(
                        toggleOption.NameKey,
                        toggleOption.DescKey,
                        toggleOption.DefaultValue,
                        toggleOption.Value,
                        OnChange(toggleOption)
                    );

                }
                else if (category.TryGetSlider(id, out var sliderOption))
                {
                    var slider = UIHelper.InstantiatePrefabUnderAnchor(__instance.SliderPrefab, __instance.ContentNode);
                    slider.Initialize(
                        sliderOption.NameKey,
                        sliderOption.DescKey,
                        sliderOption.MinValue,
                        sliderOption.MaxValue,
                        sliderOption.DefaultValue,
                        sliderOption.Value,
                        sliderOption.Decimals,
                        sliderOption.Decimals == 0,
                        OnChange(sliderOption),
                        fixedStepValue: sliderOption.StepValue
                    );
                }
                else if (category.TryGetDropdown(id, out var dropdownOption))
                {
                    var dropdown = UIHelper.InstantiatePrefabUnderAnchor(__instance.DropdownPrefab, __instance.ContentNode);
                    dropdown.Initialize(
                        dropdownOption.NameKey,
                        dropdownOption.DescKey,
                        dropdownOption.Values,
                        dropdownOption.DefaultValue,
                        dropdownOption.Value,
                        OnChange(dropdownOption)
                    );
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

    private static Action<T> OnChange<T>(Option<T> option)
    {
        return (Action<T>)(value =>
        {
            option.SetValue(value);
            OptionsManager.FullSave();
        });
    }
}
