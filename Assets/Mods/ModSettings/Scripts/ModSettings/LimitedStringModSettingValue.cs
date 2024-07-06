namespace ModSettings {
  public class LimitedStringModSettingValue {

    public string Value { get; }
    public string LocKey { get; }

    public LimitedStringModSettingValue(string value,
                                        string locKey) {
      Value = value;
      LocKey = locKey;
    }

  }
}