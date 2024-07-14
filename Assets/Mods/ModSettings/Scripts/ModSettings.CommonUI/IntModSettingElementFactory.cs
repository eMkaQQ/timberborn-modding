using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class IntModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDisplayNameProvider _modSettingDisplayNameProvider;

    public IntModSettingElementFactory(VisualElementLoader visualElementLoader,
                                       ModSettingDisplayNameProvider
                                           modSettingDisplayNameProvider) {
      _visualElementLoader = visualElementLoader;
      _modSettingDisplayNameProvider = modSettingDisplayNameProvider;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, VisualElement parent) {
      if (modSetting is ModSetting<int> intSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/IntModSettingElement");
        root.Q<Label>("SettingLabel").text = _modSettingDisplayNameProvider.Get(intSetting);
        var intField = root.Q<IntegerField>();
        intField.value = intSetting.Value;
        intField.RegisterValueChangedCallback(evt => intSetting.SetValue(evt.newValue));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}