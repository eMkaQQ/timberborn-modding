using ModSettings;
using ModSettingsUI;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ModSettingsFactoriesUI {
  internal class FloatModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public FloatModSettingElementFactory(VisualElementLoader visualElementLoader,
                                         ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public int Priority => 0;

    public bool TryCreateElement(object modSetting, VisualElement parent) {
      if (modSetting is ModSetting<float> floatModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/FloatModSettingElement");
        root.Q<Label>("SettingLabel").text = _loc.T(floatModSetting.LocKey);
        var floatField = root.Q<FloatField>();
        floatField.value = floatModSetting.Value;
        floatField.RegisterValueChangedCallback(evt => floatModSetting.SetValue(evt.newValue));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}