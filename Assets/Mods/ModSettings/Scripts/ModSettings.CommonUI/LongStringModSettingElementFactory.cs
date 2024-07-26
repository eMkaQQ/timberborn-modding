using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class LongStringModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public LongStringModSettingElementFactory(VisualElementLoader visualElementLoader,
                                              ModSettingDescriptorInitializer
                                                  modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is LongStringModSetting longStringModSetting) {
        var root =
            _visualElementLoader.LoadVisualElement("ModSettings/LongStringModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    longStringModSetting);
        var textField = root.Q<TextField>();
        textField.value = longStringModSetting.Value;
        textField.RegisterCallback<FocusOutEvent>(
            _ => longStringModSetting.SetValue(textField.value));
        element = new TextInputBaseFieldModSettingElement<string>(root, textField);
        return true;
      }
      element = null;
      return false;
    }

  }
}