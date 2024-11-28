using Timberborn.AssetSystem;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.TextureOperations;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.ColorPicker {
  public class ColorPickerFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly TextureFactory _textureFactory;
    private readonly IAssetLoader _assetLoader;
    private readonly InputService _inputService;

    public ColorPickerFactory(VisualElementLoader visualElementLoader,
                              TextureFactory textureFactory,
                              IAssetLoader assetLoader,
                              InputService inputService) {
      _visualElementLoader = visualElementLoader;
      _textureFactory = textureFactory;
      _assetLoader = assetLoader;
      _inputService = inputService;
    }

    public ColorPicker Create(Color initialColor, bool useAlpha) {
      var root = _visualElementLoader.LoadVisualElement("ModSettings/ColorPicker");

      var saturationAndValue = SetupSaturationAndValueElement(root);
      var hueSlider = SetupHueSlider(root);
      var picker = root.Q<Image>("SaturationAndValuePicker");
      picker.sprite = _assetLoader.Load<Sprite>("UI/Images/ModSettings/sv-picker");

      var saturationAndValueElement = new HSVElement(_inputService, saturationAndValue,
                                                     picker, hueSlider);

      root.Q<VisualElement>("Alpha").ToggleDisplayStyle(useAlpha);
      root.Q<VisualElement>("AlphaBackground").style.backgroundImage =
          _assetLoader.Load<Texture2D>("UI/Images/ModSettings/alpha-background");

      var rgbElement = new RGBElement(root.Q<SliderInt>("RedSlider"),
                                      root.Q<IntegerField>("RedField"),
                                      root.Q<SliderInt>("GreenSlider"),
                                      root.Q<IntegerField>("GreenField"),
                                      root.Q<SliderInt>("BlueSlider"),
                                      root.Q<IntegerField>("BlueField"),
                                      root.Q<SliderInt>("AlphaSlider"),
                                      root.Q<IntegerField>("AlphaField"));
      var colorPicker = new ColorPicker(root, saturationAndValueElement, rgbElement,
                                        root.Q<VisualElement>("InitialColorImage"),
                                        root.Q<VisualElement>("ChosenColorImage"),
                                        root.Q<TextField>("HexField"), useAlpha);
      colorPicker.Initialize(initialColor);
      return colorPicker;
    }

    private Image SetupSaturationAndValueElement(VisualElement root) {
      var saturationAndValueSettings = new TextureSettings.Builder()
          .SetSpritePreset()
          .SetFilterMode(FilterMode.Point)
          .SetSize(360, 360)
          .Build();
      var saturationAndValueTexture = _textureFactory.CreateTexture(saturationAndValueSettings);
      var saturationAndValue = root.Q<Image>("SaturationAndValue");
      saturationAndValue.image = saturationAndValueTexture;
      return saturationAndValue;
    }

    private Slider SetupHueSlider(VisualElement root) {
      var hueSettings = new TextureSettings.Builder()
          .SetSpritePreset()
          .SetFilterMode(FilterMode.Point)
          .SetSize(48, 360)
          .Build();
      var hueTexture = _textureFactory.CreateTexture(hueSettings);
      var hueBuffer = new Color[hueTexture.width * hueTexture.height];
      for (var y = 0; y < hueTexture.height; y++) {
        for (var x = 0; x < hueTexture.width; x++) {
          var hue = y / (float) (hueTexture.height - 1);
          hueBuffer[y * hueTexture.width + x] = Color.HSVToRGB(hue, 1, 1);
        }
      }

      hueTexture.SetPixels(hueBuffer);
      hueTexture.Apply();
      root.Q<Image>("HueTexture").image = hueTexture;

      var hueSlider = root.Q<Slider>("HueSlider");
      var hueSliderImage = new Image();
      var dragger = hueSlider.Q<VisualElement>("unity-dragger");
      dragger.Add(hueSliderImage);
      hueSliderImage.image = _assetLoader.Load<Sprite>("UI/Images/ModSettings/hue-picker").texture;
      hueSliderImage.AddToClassList("color-picker__hue-slider-image");
      return hueSlider;
    }

  }
}