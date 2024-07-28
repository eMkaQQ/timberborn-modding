using ModSettings.Common;
using System.Collections.Generic;
using System.Linq;
using Timberborn.DropdownSystem;

namespace ModSettings.CommonUI {
  internal class NonLocalizedLimitedStringDropdownProvider : IDropdownProvider {

    public IReadOnlyList<string> Items { get; private set; }

    private readonly LimitedStringModSetting _limitedStringModSetting;

    private NonLocalizedLimitedStringDropdownProvider(LimitedStringModSetting
                                                          limitedStringModSetting) {
      _limitedStringModSetting = limitedStringModSetting;
    }

    public static NonLocalizedLimitedStringDropdownProvider Create(LimitedStringModSetting
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

  }
}