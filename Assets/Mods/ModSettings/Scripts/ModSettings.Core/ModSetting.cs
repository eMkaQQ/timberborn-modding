using System;
using Timberborn.SettingsSystem;

namespace ModSettings.Core {
  public class ModSetting<T> : ModSetting {

    public event EventHandler<T> ValueChanged;
    public T DefaultValue { get; }
    public T Value { get; private set; }

    public ModSetting(string locKey,
                      T defaultValue) : base(locKey) {
      DefaultValue = defaultValue;
    }

    public virtual void SetValue(T value) {
      if (!value.Equals(Value)) {
        Value = value;
        ValueChanged?.Invoke(this, value);
      }
    }

    public override void Reset() {
      SetValue(DefaultValue);
    }

  }

  public abstract class ModSetting {

    public string LocKey { get; }
    public string DisplayName { get; set; }

    protected ModSetting(string locKey) {
      LocKey = locKey;
    }

    public abstract void Reset();

    public virtual bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings, string key) {
      return true;
    }

  }
}