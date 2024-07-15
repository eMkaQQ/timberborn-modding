using ModSettings.Core;
using Timberborn.SettingsSystem;
using UnityEngine;

namespace ModSettings.Common {
  public class LongStringModSetting : ModSetting<string> {

    public LongStringModSetting(string locKey, string defaultValue) : base(locKey, defaultValue) {
    }

    public override bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings) {
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