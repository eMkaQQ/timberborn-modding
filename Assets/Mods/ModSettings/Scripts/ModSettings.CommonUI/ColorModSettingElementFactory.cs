using ModSettings.ColorPicker;
using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.AssetSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class ColorModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;
    private readonly ColorPickerShower _colorPickerShower;
    private readonly IAssetLoader _assetLoader;

    public ColorModSettingElementFactory(VisualElementLoader visualElementLoader,
                                         ModSettingDescriptorInitializer
                                             modSettingDescriptorInitializer,
                                         ColorPickerShower colorPickerShower,
                                         IAssetLoader assetLoader) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
      _colorPickerShower = colorPickerShower;
      _assetLoader = assetLoader;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is ColorModSetting colorModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/ColorModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    colorModSetting);
        root.Q<VisualElement>("AlphaBackground").style.backgroundImage =
            _assetLoader.Load<Texture2D>("UI/Images/ModSettings/alpha-background");
        var colorImage = root.Q<VisualElement>("ColorImage");
        colorImage.style.backgroundColor = colorModSetting.Color;
        colorImage.RegisterCallback<ClickEvent>(
            _ => _colorPickerShower.ShowColorPicker(colorModSetting.Color, colorModSetting.UseAlpha,
                                                    newColor => {
                                                      colorImage.style.backgroundColor = newColor;
                                                      colorModSetting.SetValue(newColor);
                                                    }));
        colorModSetting.ValueChanged +=
            (_, _) => colorImage.style.backgroundColor = colorModSetting.Color;
        element = new ModSettingElement(root, modSetting);
        return true;
      }
      element = null;
      return false;
    }

  }
}