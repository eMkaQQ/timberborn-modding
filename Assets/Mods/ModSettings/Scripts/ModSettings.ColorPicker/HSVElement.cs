using System;
using Timberborn.InputSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace ModSettings.ColorPicker {
  public class HSVElement {

    private static readonly float HueRange = 360f;
    public event EventHandler<Color> HSVChanged;
    private readonly InputService _inputService;
    private readonly Image _svElement;
    private readonly VisualElement _svPicker;
    private readonly Slider _hueSlider;
    private Texture2D _svTexture;
    private Texture _hueTexture;
    private bool _isMouseDown;
    private Color[] _buffer;

    public HSVElement(InputService inputService,
                      Image svElement,
                      VisualElement svPicker,
                      Slider hueSlider) {
      _inputService = inputService;
      _svElement = svElement;
      _svPicker = svPicker;
      _hueSlider = hueSlider;
    }

    public void Initialize(Color initialColor) {
      _svElement.RegisterCallback<MouseDownEvent>(OnMouseDown);
      _hueSlider.RegisterValueChangedCallback(OnHueChanged);
      _svTexture = (Texture2D) _svElement.image;
      _hueTexture = ((Image) _hueSlider.parent).image;
      _buffer = new Color[_svTexture.width * _svTexture.height];
      SetColor(initialColor);
    }

    public bool UpdateInput() {
      if (_isMouseDown) {
        if (_inputService.MainMouseButtonHeld) {
          UpdateSaturationAndValue();
        } else {
          _isMouseDown = false;
        }
      }
      return false;
    }

    public void SetColor(Color color) {
      Color.RGBToHSV(color, out var hue, out var saturation, out var value);
      _hueSlider.SetValueWithoutNotify(hue * HueRange);
      UpdateSaturationAndValueTexture(hue);
      SetPickerPosition(saturation, value);
    }

    public void Clear() {
      Object.Destroy(_svTexture);
      Object.Destroy(_hueTexture);
    }

    private void OnHueChanged(ChangeEvent<float> evt) {
      UpdateSaturationAndValueTexture(evt.newValue / HueRange);
      NotifyHSVChanged();
    }

    private void UpdateSaturationAndValueTexture(float hue) {
      for (var y = 0; y < _svTexture.height; y++) {
        for (var x = 0; x < _svTexture.width; x++) {
          var saturation = x / (float) (_svTexture.width - 1);
          var value = y / (float) (_svTexture.height - 1);
          var pixelColor = Color.HSVToRGB(hue, saturation, value);
          _buffer[y * _svTexture.width + x] = pixelColor;
        }
      }
      _svTexture.SetPixels(_buffer);
      _svTexture.Apply();
    }

    private void SetPickerPosition(float saturation, float value) {
      _svPicker.style.left = Length.Percent(saturation * 100);
      _svPicker.style.top = Length.Percent((1 - value) * 100);
    }

    private void OnMouseDown(MouseDownEvent evt) {
      UpdateSaturationAndValue();
      _isMouseDown = true;
    }

    private void UpdateSaturationAndValue() {
      var mousePosNdc = _inputService.MousePositionNdc;
      var rootRect = _svElement.panel.visualTree.worldBound;
      var rootMousePos = new Vector2(mousePosNdc.x * rootRect.width,
                                     (1 - mousePosNdc.y) * rootRect.height);
      var localPos = _svElement.WorldToLocal(rootMousePos);
      localPos.x = Mathf.Clamp(localPos.x, 0, _svElement.contentRect.width);
      localPos.y = Mathf.Clamp(localPos.y, 0, _svElement.contentRect.height);
      _svPicker.style.left = localPos.x;
      _svPicker.style.top = localPos.y;
      NotifyHSVChanged();
    }

    private void NotifyHSVChanged() {
      var saturation = _svPicker.style.left.value.value / _svElement.contentRect.width;
      var value = 1 - _svPicker.style.top.value.value / _svElement.contentRect.height;
      var color = Color.HSVToRGB(_hueSlider.value / HueRange, saturation, value);
      HSVChanged?.Invoke(this, color);
    }

  }
}