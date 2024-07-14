using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class BoolModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDisplayNameProvider _modSettingDisplayNameProvider;

    public BoolModSettingElementFactory(VisualElementLoader visualElementLoader,
                                        ModSettingDisplayNameProvider
                                            modSettingDisplayNameProvider) {
      _visualElementLoader = visualElementLoader;
      _modSettingDisplayNameProvider = modSettingDisplayNameProvider;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ModSetting<bool> boolModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/BoolModSettingElement");
        root.Q<Label>("SettingLabel").text = _modSettingDisplayNameProvider.Get(boolModSetting);
        var toggle = root.Q<Toggle>();
        toggle.value = boolModSetting.Value;
        toggle.RegisterValueChangedCallback(evt => boolModSetting.SetValue(evt.newValue));
        element = new ModSettingElement(root);
        return true;
      }
      element = null;
      return false;
    }

  }
}