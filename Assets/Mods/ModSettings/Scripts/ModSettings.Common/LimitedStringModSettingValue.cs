namespace ModSettings.Common {
  public class LimitedStringModSettingValue : ILimitedStringModSettingValue {

    public string Value { get; }
    public string Key { get; }

    public LimitedStringModSettingValue(string value,
                                        string locKey) {
      Value = value;
      Key = locKey;
    }
    

  }
}