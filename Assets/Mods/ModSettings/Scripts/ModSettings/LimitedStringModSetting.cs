using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ModSettings {
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
        if(limitedStringModSettingValue.Value == value) {
          base.SetValue(value);
          return;
        }
      }
      throw new ArgumentException($"Trying to set invalid value ({value}) for setting {LocKey}");
    }

  }
}