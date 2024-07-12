using ModSettings.Core;
using UnityEngine;

namespace ModSettings.Common {
  public class RangeIntModSetting : ModSetting<int> {

    public int MinValue { get; }
    public int MaxValue { get; }

    public RangeIntModSetting(string locKey,
                              int defaultValue,
                              int minValue,
                              int maxValue) : base(locKey, defaultValue) {
      MinValue = minValue;
      MaxValue = maxValue;
    }

    public override void SetValue(int value) {
      base.SetValue(Mathf.Clamp(value, MinValue, MaxValue));
    }

  }
}