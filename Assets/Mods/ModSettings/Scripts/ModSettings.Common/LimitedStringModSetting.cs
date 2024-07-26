using ModSettings.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Timberborn.SettingsSystem;

namespace ModSettings.Common {
  public class LimitedStringModSetting : ModSetting<string> {

    public ImmutableArray<LimitedStringModSettingValue> Values { get; }

    [Obsolete("Use constructor with ModSettingDescriptor parameter instead.")]
    public LimitedStringModSetting(string locKey,
                                   int defaultOptionIndex,
                                   IList<LimitedStringModSettingValue> values)
        : base(locKey, values[defaultOptionIndex].Value) {
      Values = values.ToImmutableArray();
    }

    public LimitedStringModSetting(int defaultOptionIndex,
                                   IList<LimitedStringModSettingValue> values,
                                   ModSettingDescriptor descriptor)
        : base(values[defaultOptionIndex].Value, descriptor) {
      Values = values.ToImmutableArray();
    }

    public override void SetValue(string value) {
      foreach (var limitedStringModSettingValue in Values) {
        if (limitedStringModSettingValue.Value == value) {
          base.SetValue(value);
          return;
        }
      }
      throw new ArgumentException(
          $"Trying to set invalid value ({value}) for {nameof(LimitedStringModSetting)}. "
          + $"Available values: {string.Join(", ", Values)}");
    }

    public override bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings,
                                 string key) {
      var value = settings.GetString(key, null);
      if (value != null) {
        foreach (var limitedStringModSettingValue in Values) {
          if (limitedStringModSettingValue.Value == value) {
            return true;
          }
        }
        settings.Clear(key);
      }
      return true;
    }

  }
}