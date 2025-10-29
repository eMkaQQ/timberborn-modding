using System;
using Timberborn.SettingsSystem;

namespace ModSettings.Core {
  public class ModSetting<T> : ModSetting {

    public event EventHandler<T> ValueChanged;
    public T DefaultValue { get; }
    public T Value { get; private set; }
    
    public ModSetting(T defaultValue,
                      ModSettingDescriptor descriptor) : base(descriptor) {
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

    public ModSettingDescriptor Descriptor { get; private set; }

    protected ModSetting(ModSettingDescriptor descriptor) {
      Descriptor = descriptor;
    }
    
    public abstract void Reset();

    public virtual bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings, string key) {
      return true;
    }

  }
}