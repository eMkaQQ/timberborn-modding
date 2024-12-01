using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.ColorPicker {
  public class ColorPicker {

    public VisualElement Root { get; }
    public Color ChosenColor { get; private set; }
    private readonly HSVElement _hsvElement;
    private readonly RGBElement _rgbElement;
    private readonly VisualElement _initialColorImage;
    private readonly VisualElement _chosenColorImage;
    private readonly TextField _hexField;
    private readonly bool _useAlpha;

    public ColorPicker(VisualElement root,
                       HSVElement hsvElement,
                       RGBElement rgbElement,
                       VisualElement initialColorImage,
                       VisualElement chosenColorImage,
                       TextField hexField,
                       bool useAlpha) {
      Root = root;
      _hsvElement = hsvElement;
      _rgbElement = rgbElement;
      _initialColorImage = initialColorImage;
      _chosenColorImage = chosenColorImage;
      _hexField = hexField;
      _useAlpha = useAlpha;
    }

    public void Initialize(Color initialColor) {
      _initialColorImage.style.backgroundColor = initialColor;
      _hsvElement.Initialize(initialColor);
      _rgbElement.Initialize(initialColor);
      UpdateChosenColor(initialColor);
      UpdateHexField(initialColor);
      _hsvElement.HSVChanged += OnHSVChanged;
      _rgbElement.RGBChanged += OnRGBChanged;
      _hexField.RegisterCallback<FocusOutEvent>(OnHexFieldChanged);
    }

    public void Update() {
      _hsvElement.Update();
    }

    public void Clear() {
      _hsvElement.Clear();
    }

    private void OnHSVChanged(object _, Color color) {
      color.a = _useAlpha ? ChosenColor.a : 1;
      UpdateRGBColor(color);
      UpdateChosenColor(color);
      UpdateHexField(color);
    }

    private void OnRGBChanged(object _, Color color) {
      UpdateHSVColor(color);
      UpdateChosenColor(color);
      UpdateHexField(color);
    }

    private void OnHexFieldChanged(FocusOutEvent evt) {
      if (ColorUtility.TryParseHtmlString($"#{_hexField.value}", out var color)) {
        if (!_useAlpha) {
          color.a = 1;
        }
        UpdateHSVColor(color);
        UpdateRGBColor(color);
        UpdateChosenColor(color);
        UpdateHexField(color);
      } else {
        UpdateHexField(ChosenColor);
      }
    }

    private void UpdateRGBColor(Color color) {
      _rgbElement.SetColor(color);
    }

    private void UpdateHSVColor(Color color) {
      _hsvElement.SetColor(color);
    }

    private void UpdateChosenColor(Color color) {
      ChosenColor = color;
      _chosenColorImage.style.backgroundColor = color;
    }

    private void UpdateHexField(Color color) {
      _hexField.value = _useAlpha
          ? ColorUtility.ToHtmlStringRGBA(color)
          : ColorUtility.ToHtmlStringRGB(color);
    }

  }
}