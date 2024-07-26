using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using System.Globalization;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class SliderIntModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public SliderIntModSettingElementFactory(VisualElementLoader visualElementLoader,
                                             ModSettingDescriptorInitializer
                                                 modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is RangeIntModSetting rangeIntModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/SliderIntModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    rangeIntModSetting);
        InitializeSlider(root, rangeIntModSetting);
        element = new ModSettingElement(root);
        return true;
      }
      element = null;
      return false;
    }

    private static void InitializeSlider(VisualElement root,
                                         RangeIntModSetting rangeIntModSetting) {
      var valueLabel = root.Q<Label>("Value");
      var slider = root.Q<Slider>("Slider");
      slider.lowValue = rangeIntModSetting.MinValue;
      slider.highValue = rangeIntModSetting.MaxValue;
      slider.value = rangeIntModSetting.Value;
      slider.RegisterValueChangedCallback(v => {
        rangeIntModSetting.SetValue(Mathf.RoundToInt(v.newValue));
        valueLabel.text = rangeIntModSetting.Value.ToString(CultureInfo.InvariantCulture);
      });
      valueLabel.text = rangeIntModSetting.Value.ToString(CultureInfo.InvariantCulture);
    }

  }
}