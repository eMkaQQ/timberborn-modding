using ModSettings.Common;
using ModSettings.CoreUI;
using System.Globalization;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class SliderIntModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public SliderIntModSettingElementFactory(VisualElementLoader visualElementLoader,
                                             ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public int Priority => 100;

    public bool TryCreateElement(object modSetting, VisualElement parent) {
      if (modSetting is RangeIntModSetting rangeIntModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/SliderIntModSettingElement");
        root.Q<Label>("SettingLabel").text = _loc.T(rangeIntModSetting.LocKey);
        InitializeSlider(root, rangeIntModSetting);
        parent.Add(root);
        return true;
      }
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