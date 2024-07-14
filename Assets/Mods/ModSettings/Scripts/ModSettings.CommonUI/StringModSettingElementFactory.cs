using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class StringModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDisplayNameProvider _modSettingDisplayNameProvider;

    public StringModSettingElementFactory(VisualElementLoader visualElementLoader,
                                          ModSettingDisplayNameProvider
                                              modSettingDisplayNameProvider) {
      _visualElementLoader = visualElementLoader;
      _modSettingDisplayNameProvider = modSettingDisplayNameProvider;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, VisualElement parent) {
      if (modSetting is ModSetting<string> stringModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/StringModSettingElement");
        root.Q<Label>("SettingLabel").text = _modSettingDisplayNameProvider.Get(stringModSetting);
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