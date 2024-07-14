using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class FloatModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDisplayNameProvider _modSettingDisplayNameProvider;

    public FloatModSettingElementFactory(VisualElementLoader visualElementLoader,
                                         ModSettingDisplayNameProvider
                                             modSettingDisplayNameProvider) {
      _visualElementLoader = visualElementLoader;
      _modSettingDisplayNameProvider = modSettingDisplayNameProvider;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, VisualElement parent) {
      if (modSetting is ModSetting<float> floatModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/FloatModSettingElement");
        root.Q<Label>("SettingLabel").text = _modSettingDisplayNameProvider.Get(floatModSetting);
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