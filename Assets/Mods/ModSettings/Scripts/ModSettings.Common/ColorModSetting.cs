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

    public ColorModSetting(Color defaultValue,
                           ModSettingDescriptor descriptor,
                           bool useAlpha) : base(ToHtmlString(defaultValue, useAlpha), descriptor) {
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
      if (!TrySetColor(value)) {
        Debug.LogWarning($"Failed to parse color value: {value}");
        TrySetColor(DefaultValue);
      }
      base.SetValue(ToHtmlString(Color, UseAlpha));
    }

    public void SetValue(Color color) {
      SetValue(ToHtmlString(color, UseAlpha));
    }

    private static string ToHtmlString(Color color, bool useAlpha) {
      return useAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color);
    }

    private static bool TryParseColor(string value, out Color color) {
      return ColorUtility.TryParseHtmlString($"#{value}", out color);
    }

    private bool TrySetColor(string value) {
      if (TryParseColor(value, out var color)) {
        if (!UseAlpha) {
          color.a = 1;
        }
        Color = color;
        return true;
      }
      return false;
    }

  }
}