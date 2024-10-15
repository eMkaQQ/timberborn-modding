using ModSettings.Core;
using Timberborn.SettingsSystem;
using UnityEngine;

namespace ModSettings.Common {
  public class ColorModSetting : ModSetting<string> {

    public Color Color { get; private set; }
    public bool UseAlpha { get; }

    public ColorModSetting(string defaultValue,
                           ModSettingDescriptor descriptor,
                           bool useAlpha) : base(defaultValue, descriptor) {
      UseAlpha = useAlpha;
    }

    public override bool IsValid(ModSettingsOwner modSettingsOwner, ISettings settings,
                                 string key) {
      var value = settings.GetString(key, null);
      if (!TryParseColor(value, out _)) {
        settings.Clear(key);
      }
      return true;
    }

    public override void SetValue(string value) {
      if (TryParseColor(value, out var color)) {
        Color = color;
      } else {
        value = DefaultValue;
        Debug.LogWarning($"Failed to parse color value: {value}");
      }
      base.SetValue(value);
    }

    public void SetValue(Color color) {
      if (UseAlpha) {
        SetValue($"{ColorUtility.ToHtmlStringRGBA(color)}");
      } else {
        SetValue($"{ColorUtility.ToHtmlStringRGB(color)}");
      }
    }

    private static bool TryParseColor(string value, out Color color) {
      return ColorUtility.TryParseHtmlString($"#{value}", out color);
    }

  }
}