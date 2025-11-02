using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class ReadonlyTextModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public ReadonlyTextModSettingElementFactory(VisualElementLoader visualElementLoader,
                                                ModSettingDescriptorInitializer
                                                    modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ReadonlyTextModSetting readonlyTextModSetting) {
        var root =
            _visualElementLoader.LoadVisualElement("ModSettings/ReadonlyTextModSettingElement");
        var descriptor = root.Q<VisualElement>("Descriptor");
        _modSettingDescriptorInitializer.Initialize(descriptor, readonlyTextModSetting);
        descriptor.style.unityTextAlign = readonlyTextModSetting.Settings.TextAlignment;
        descriptor.style.fontSize = readonlyTextModSetting.Settings.FontSize;
        descriptor.style.marginLeft = readonlyTextModSetting.Settings.Margin.x;
        descriptor.style.marginRight = readonlyTextModSetting.Settings.Margin.y;
        descriptor.style.marginTop = readonlyTextModSetting.Settings.Margin.z;
        descriptor.style.marginBottom = readonlyTextModSetting.Settings.Margin.w;
        element = new ModSettingElement(root, modSetting);
        return true;
      }
      element = null;
      return false;
    }

  }
}