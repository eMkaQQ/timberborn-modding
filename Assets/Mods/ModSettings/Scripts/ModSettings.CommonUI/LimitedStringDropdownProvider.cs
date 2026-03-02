using ModSettings.Common;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Timberborn.DropdownSystem;
using UnityEngine;

namespace ModSettings.CommonUI {
  internal class LimitedStringDropdownProvider : IExtendedDropdownProvider {

    public IReadOnlyList<string> Items { get; private init; }

    private readonly LimitedStringModSetting _limitedStringModSetting;

    private LimitedStringDropdownProvider(LimitedStringModSetting limitedStringModSetting) {
      _limitedStringModSetting = limitedStringModSetting;
    }

    public static LimitedStringDropdownProvider Create(LimitedStringModSetting
                                                           limitedStringModSetting) {
      return new(limitedStringModSetting) {
          Items = limitedStringModSetting.Values.Select(value => value.Key).ToList()
      };
    }

    public string GetValue() {
      return _limitedStringModSetting.Values
          .Single(value => value.Value == _limitedStringModSetting.Value).Key;
    }

    public void SetValue(string value) {
      _limitedStringModSetting.SetValue(_limitedStringModSetting.Values
                                            .Single(v => v.Key == value).Value);
    }

    public string FormatDisplayText(string value, bool selected) {
      return value;
    }

    public Sprite GetIcon(string value) {
      return null;
    }

    public ImmutableArray<string> GetItemClasses(string value) {
      return ImmutableArray<string>.Empty;
    }

  }
}