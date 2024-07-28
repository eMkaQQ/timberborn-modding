namespace ModSettings.Common {
  public class NonLocalizedLimitedStringModSettingValue : ILimitedStringModSettingValue {

    public string Value { get; }
    public string Key => Value;

    public NonLocalizedLimitedStringModSettingValue(string value) {
      Value = value;
    }

  }
}