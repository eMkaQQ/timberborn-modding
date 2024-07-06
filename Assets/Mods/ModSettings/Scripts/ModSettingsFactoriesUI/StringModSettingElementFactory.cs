using ModSettings;
using ModSettingsUI;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ModSettingsFactoriesUI {
  internal class StringModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public StringModSettingElementFactory(VisualElementLoader visualElementLoader,
                                          ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public int Priority => 0;

    public bool TryCreateElement(object modSetting, VisualElement parent) {
      if (modSetting is ModSetting<string> stringModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/StringModSettingElement");
        root.Q<Label>("SettingLabel").text = _loc.T(stringModSetting.LocKey);
        var textField = root.Q<TextField>();
        textField.value = stringModSetting.Value;
        textField.RegisterValueChangedCallback(evt => stringModSetting.SetValue(evt.newValue));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}