using ModSettings.Core;
using System;
using UnityEngine;

namespace ModSettings.Common {
  public class RangeIntModSetting : ModSetting<int> {

    public int MinValue { get; }
    public int MaxValue { get; }

    [Obsolete("Use constructor with ModSettingDescriptor parameter instead.")]
    public RangeIntModSetting(string locKey,
                              int defaultValue,
                              int minValue,
                              int maxValue) : base(locKey, defaultValue) {
      MinValue = minValue;
      MaxValue = maxValue;
    }

    public RangeIntModSetting(int defaultValue,
                              int minValue,
                              int maxValue,
                              ModSettingDescriptor descriptor) : base(defaultValue, descriptor) {
      MinValue = minValue;
      MaxValue = maxValue;
    }

    public override void SetValue(int value) {
      base.SetValue(Mathf.Clamp(value, MinValue, MaxValue));
    }

  }
}