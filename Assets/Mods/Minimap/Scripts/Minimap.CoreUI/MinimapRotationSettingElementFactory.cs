using ModSettings.CommonUI;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace Minimap.CoreUI {
  internal class MinimapRotationSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;

    public MinimapRotationSettingElementFactory(VisualElementLoader visualElementLoader,
                                                ModSettingDescriptorInitializer
                                                    modSettingDescriptorInitializer) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
    }

    public int Priority => 0;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is MinimapRotationSetting rotationSetting) {
        var root = _visualElementLoader.LoadVisualElement("Minimap/RotateMapSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    rotationSetting);
        root.Q<Button>("RotateClockwiseButton")
            .RegisterCallback<ClickEvent>(_ => rotationSetting.RotateClockwise());
        root.Q<Button>("RotateCounterClockwiseButton")
            .RegisterCallback<ClickEvent>(_ => rotationSetting.RotateCounterClockwise());
        element = new ModSettingElement(root, modSetting);
        return true;
      }

      element = null;
      return false;
    }

  }
}