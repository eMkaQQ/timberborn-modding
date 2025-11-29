using SecondShift.Core;
using Timberborn.AssetSystem;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.SliderToggleSystem;
using Timberborn.TimeSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace SecondShift.CoreUI {
  internal class TwoShiftsWorkersToggle {

    private static readonly string ShowFirstShiftLocKey = "SecondShift.ShowFirstShift";
    private static readonly string ShowSecondShiftLocKey = "SecondShift.ShowSecondShift";
    public bool FirstShiftShown { get; private set; }
    private readonly SliderToggleFactory _sliderToggleFactory;
    private readonly ILoc _loc;
    private readonly IAssetLoader _assetLoader;
    private readonly IDayNightCycle _dayNightCycle;
    private SliderToggle _sliderToggle;

    public TwoShiftsWorkersToggle(SliderToggleFactory sliderToggleFactory,
                                  ILoc loc,
                                  IAssetLoader assetLoader,
                                  IDayNightCycle dayNightCycle) {
      _sliderToggleFactory = sliderToggleFactory;
      _loc = loc;
      _assetLoader = assetLoader;
      _dayNightCycle = dayNightCycle;
    }

    public void Initialize(VisualElement parent) {
      Asserts.FieldIsNull(this, _sliderToggle, nameof(_sliderToggle));
      var firstShiftSprite = _assetLoader.Load<Sprite>("UI/Images/Game/first-shift");
      var secondShiftSprite = _assetLoader.Load<Sprite>("UI/Images/Game/second-shift");
      var firstShiftSliderItem = SliderToggleItem.Create(GetFirstShiftTooltip, firstShiftSprite,
                                                         ShowFirstShift,
                                                         () => FirstShiftShown);
      var secondShiftSliderItem = SliderToggleItem.Create(GetSecondShiftTooltip, secondShiftSprite,
                                                          ShowSecondShift,
                                                          () => !FirstShiftShown);
      _sliderToggle =
          _sliderToggleFactory.Create(parent, firstShiftSliderItem, secondShiftSliderItem);
    }

    public void Open(TwoShiftsWorkplace twoShiftsWorkplace) {
      if (twoShiftsWorkplace.TwoShiftsEnabled) {
        Show();
      } else {
        Hide();
      }
    }

    public void Update() {
      _sliderToggle.Update();
    }

    public void Show() {
      FirstShiftShown = _dayNightCycle.DayProgress <= 0.5f;
      _sliderToggle.Root.ToggleDisplayStyle(true);
    }

    public void Hide() {
      _sliderToggle.Root.ToggleDisplayStyle(false);
    }

    private string GetFirstShiftTooltip() {
      return _loc.T(ShowFirstShiftLocKey);
    }

    private string GetSecondShiftTooltip() {
      return _loc.T(ShowSecondShiftLocKey);
    }

    private void ShowFirstShift() {
      FirstShiftShown = true;
    }

    private void ShowSecondShift() {
      FirstShiftShown = false;
    }

  }
}