using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class IntModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public IntModSettingElementFactory(VisualElementLoader visualElementLoader,
                                       ModSettingDescriptorInitializer
                                           modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ModSetting<int> intSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/IntModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    intSetting);
        var intField = root.Q<IntegerField>();
        intField.value = intSetting.Value;
        intField.RegisterCallback<FocusOutEvent>(_ => intSetting.SetValue(intField.value));
        element = new TextInputBaseFieldModSettingElement<int>(root, intField);
        return true;
      }
      element = null;
      return false;
    }

  }
}