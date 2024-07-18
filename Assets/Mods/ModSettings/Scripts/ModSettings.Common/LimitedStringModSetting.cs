using ModSettings.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Timberborn.SettingsSystem;
using UnityEngine;

namespace ModSettings.Common {
  public class LimitedStringModSetting : ModSetting<string> {

    public ImmutableArray<LimitedStringModSettingValue> Values { get; }

    public LimitedStringModSetting(string locKey,
                                   int defaultOptionIndex,
                                   IList<LimitedStringModSettingValue> values)
        : base(locKey, values[defaultOptionIndex].Value) {
      Values = values.ToImmutableArray();
    }

    public override void SetValue(string value) {
      foreach (var limitedStringModSettingValue in Values) {
        if (limitedStringModSettingValue.Value == value) {
          base.SetValue(value);
          return;
        }
      }
      throw new ArgumentException($"Trying to set invalid value ({value}) for setting {LocKey}");
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