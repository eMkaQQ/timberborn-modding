using System;

namespace ModSettings.Core {
  public class ModSetting<T> {

    public event EventHandler<T> ValueChanged;
    public string LocKey { get; }
    public T DefaultValue { get; }
    public T Value { get; private set; }
    public string DisplayName { get; set; }

    public ModSetting(string locKey,
                      T defaultValue) {
      LocKey = locKey;
      DefaultValue = defaultValue;
    }

    public virtual void SetValue(T value) {
      if (!value.Equals(Value)) {
        Value = value;
        ValueChanged?.Invoke(this, value);
      }
    }

  }
}