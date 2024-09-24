using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class BoolModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public BoolModSettingElementFactory(VisualElementLoader visualElementLoader,
                                        ModSettingDescriptorInitializer
                                            modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ModSetting<bool> boolModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/BoolModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    boolModSetting);
        var toggle = root.Q<Toggle>();
        toggle.value = boolModSetting.Value;
        toggle.RegisterValueChangedCallback(evt => boolModSetting.SetValue(evt.newValue));
        element = new ModSettingElement(root, modSetting);
        return true;
      }
      element = null;
      return false;
    }

  }
}