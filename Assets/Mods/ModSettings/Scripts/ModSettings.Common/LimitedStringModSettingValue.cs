using System;

namespace ModSettings.Common {
  public class LimitedStringModSettingValue {

    public string Value { get; }
    public string LocKey { get; }
    public string DisplayName { get; }

    [Obsolete("Use Create method instead.")]
    public LimitedStringModSettingValue(string value,
                                        string locKey) {
      Value = value;
      LocKey = locKey;
    }

    private LimitedStringModSettingValue(string value,
                                         string locKey,
                                         string displayName) {
      Value = value;
      LocKey = locKey;
      DisplayName = displayName;
    }

    public static LimitedStringModSettingValue CreateLocalized(string value,
                                                               string locKey) {
      return new(value, locKey, null);
    }

    public static LimitedStringModSettingValue Create(string value,
                                                      string displayName) {
      return new(value, null, displayName);
    }

  }
}