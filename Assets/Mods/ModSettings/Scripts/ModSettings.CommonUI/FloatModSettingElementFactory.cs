using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class FloatModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public FloatModSettingElementFactory(VisualElementLoader visualElementLoader,
                                         ModSettingDescriptorInitializer
                                             modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ModSetting<float> floatModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/FloatModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    floatModSetting);
        var floatField = root.Q<FloatField>();
        floatField.value = floatModSetting.Value;
        floatField.RegisterCallback<FocusOutEvent>(_ => floatModSetting.SetValue(floatField.value));
        element = new TextInputBaseFieldModSettingElement<float>(root, floatField);
        return true;
      }
      element = null;
      return false;
    }

  }
}