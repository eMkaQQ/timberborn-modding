using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class StringModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public StringModSettingElementFactory(VisualElementLoader visualElementLoader,
                                          ModSettingDescriptorInitializer
                                              modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ModSetting<string> stringModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/StringModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    stringModSetting);
        var textField = root.Q<TextField>();
        textField.value = stringModSetting.Value;
        textField.RegisterCallback<FocusOutEvent>(_ => stringModSetting.SetValue(textField.value));
        element = new TextInputBaseFieldModSettingElement<string>(root, textField);
        return true;
      }
      element = null;
      return false;
    }

  }
}