using ModSettings.Common;
using System.Collections.Generic;
using System.Linq;
using Timberborn.DropdownSystem;
using UnityEngine;

namespace ModSettings.CommonUI {
  internal class LimitedStringDropdownProvider : IExtendedDropdownProvider {

    public IReadOnlyList<string> Items { get; private set; }

    private readonly LimitedStringModSetting _limitedStringModSetting;

    private LimitedStringDropdownProvider(LimitedStringModSetting limitedStringModSetting) {
      _limitedStringModSetting = limitedStringModSetting;
    }

    public static LimitedStringDropdownProvider Create(LimitedStringModSetting
                                                           limitedStringModSetting) {
      return new(limitedStringModSetting) {
          Items = limitedStringModSetting.Values.Select(value => value.LocKey).ToList()
      };
    }

    public string GetValue() {
      return _limitedStringModSetting.Values
          .Single(value => value.Value == _limitedStringModSetting.Value).LocKey;
    }

    public void SetValue(string value) {
      _limitedStringModSetting.SetValue(_limitedStringModSetting.Values
                                            .Single(v => v.LocKey == value).Value);
    }

    public string FormatDisplayText(string value) {
      return value;
    }

    public Sprite GetIcon(string value) {
      return null;
    }

  }
}