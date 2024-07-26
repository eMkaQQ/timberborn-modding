using ModSettings.Core;
using System;
using Timberborn.SettingsSystem;
using UnityEngine;

namespace ModSettings.Common {
  public class LongStringModSetting : ModSetting<string> {

    [Obsolete("Use constructor with ModSettingDescriptor parameter instead.")]
    public LongStringModSetting(string locKey,
                                string defaultValue) : base(locKey, defaultValue) {
    }

    public LongStringModSetting(string defaultValue,
                                ModSettingDescriptor descriptor) : base(defaultValue, descriptor) {
    }

    public override bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings,
                                 string key) {
      if (settings.GetType().Assembly == typeof(ISettings).Assembly) {
        Debug.LogWarning(
            $"Using {nameof(LongStringModSetting)} with {nameof(Timberborn.SettingsSystem)} "
            + $"is not supported. Use the {nameof(DefaultModFileStoredSettings)} "
            + $"or other custom implementation of {nameof(ISettings)} instead.");
        return false;
      }
      return true;
    }

  }
}