using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class LongStringModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDisplayNameProvider _modSettingDisplayNameProvider;

    public LongStringModSettingElementFactory(VisualElementLoader visualElementLoader,
                                              ModSettingDisplayNameProvider
                                                  modSettingDisplayNameProvider) {
      _visualElementLoader = visualElementLoader;
      _modSettingDisplayNameProvider = modSettingDisplayNameProvider;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, VisualElement parent) {
      if (modSetting is LongStringModSetting longStringModSetting) {
        var root =
            _visualElementLoader.LoadVisualElement("ModSettings/LongStringModSettingElement");
        root.Q<Label>("SettingLabel").text =
            _modSettingDisplayNameProvider.Get(longStringModSetting);
        var textField = root.Q<TextField>();
        textField.value = longStringModSetting.Value;
        textField.RegisterCallback<FocusOutEvent>(
            _ => longStringModSetting.SetValue(textField.value));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}