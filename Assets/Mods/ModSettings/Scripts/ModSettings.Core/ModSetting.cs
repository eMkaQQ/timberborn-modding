using System;
using Timberborn.SettingsSystem;

namespace ModSettings.Core {
  public class ModSetting<T> : ModSetting {

    public event EventHandler<T> ValueChanged;
    public T DefaultValue { get; }
    public T Value { get; private set; }

    [Obsolete("Use constructor with ModSettingDescriptor parameter instead.")]
    public ModSetting(string locKey,
                      T defaultValue) : base(locKey) {
      DefaultValue = defaultValue;
    }

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

    [Obsolete("Use constructor with ModSettingDescriptor parameter instead.")]
    protected ModSetting(string locKey) {
      Descriptor = ModSettingDescriptor.CreateLocalized(locKey);
    }

    protected ModSetting(ModSettingDescriptor descriptor) {
      Descriptor = descriptor;
    }

    [Obsolete("Use Descriptor.Name instead.")]
    public string DisplayName {
      get => Descriptor.Name;
      set => Descriptor = ModSettingDescriptor.Create(value);
    }

    [Obsolete("Use Descriptor.NameLocKey instead.")]
    public string LocKey => Descriptor.NameLocKey;

    public abstract void Reset();

    public virtual bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings, string key) {
      return true;
    }

  }
}