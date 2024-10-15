using System;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.ColorPicker {
  public class RGBElement {

    private static readonly float RGBMax = 255;

    public event EventHandler<Color> RGBChanged;
    private readonly SliderInt _redSlider;
    private readonly IntegerField _redField;
    private readonly SliderInt _greenSlider;
    private readonly IntegerField _greenField;
    private readonly SliderInt _blueSlider;
    private readonly IntegerField _blueField;
    private readonly SliderInt _alphaSlider;
    private readonly IntegerField _alphaField;

    public RGBElement(SliderInt redSlider,
                      IntegerField redField,
                      SliderInt greenSlider,
                      IntegerField greenField,
                      SliderInt blueSlider,
                      IntegerField blueField,
                      SliderInt alphaSlider,
                      IntegerField alphaField) {
      _redSlider = redSlider;
      _redField = redField;
      _greenSlider = greenSlider;
      _greenField = greenField;
      _blueSlider = blueSlider;
      _blueField = blueField;
      _alphaSlider = alphaSlider;
      _alphaField = alphaField;
    }

    public void Initialize(Color initialColor) {
      InitializeSlider(_redSlider, _redField);
      InitializeSlider(_greenSlider, _greenField);
      InitializeSlider(_blueSlider, _blueField);
      InitializeSlider(_alphaSlider, _alphaField);
      SetColor(initialColor);
    }

    public void SetColor(Color color) {
      var redValue = Mathf.RoundToInt(color.r * RGBMax);
      _redSlider.SetValueWithoutNotify(redValue);
      _redField.SetValueWithoutNotify(redValue);
      var greenValue = Mathf.RoundToInt(color.g * RGBMax);
      _greenSlider.SetValueWithoutNotify(greenValue);
      _greenField.SetValueWithoutNotify(greenValue);
      var blueValue = Mathf.RoundToInt(color.b * RGBMax);
      _blueSlider.SetValueWithoutNotify(blueValue);
      _blueField.SetValueWithoutNotify(blueValue);
      var alphaValue = Mathf.RoundToInt(color.a * RGBMax);
      _alphaSlider.SetValueWithoutNotify(alphaValue);
      _alphaField.SetValueWithoutNotify(alphaValue);
    }

    public bool UpdateInput() {
      return _redField.IsFocused()
             || _greenField.IsFocused()
             || _blueField.IsFocused()
             || _alphaField.IsFocused();
    }

    private void InitializeSlider(SliderInt slider, IntegerField field) {
      slider.RegisterValueChangedCallback(evt => {
        field.SetValueWithoutNotify(evt.newValue);
        NotifyRGBChanged();
      });
      field.RegisterCallback<FocusOutEvent>(_ => {
        var clampedValue = Mathf.Clamp(field.value, 0, (int) RGBMax);
        field.SetValueWithoutNotify(clampedValue);
        slider.SetValueWithoutNotify(clampedValue);
        NotifyRGBChanged();
      });
    }

    private void NotifyRGBChanged() {
      var color = new Color(_redSlider.value / RGBMax, _greenSlider.value / RGBMax,
                            _blueSlider.value / RGBMax, _alphaSlider.value / RGBMax);
      RGBChanged?.Invoke(this, color);
    }

  }
}