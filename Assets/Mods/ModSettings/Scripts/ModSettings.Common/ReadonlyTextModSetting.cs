using JetBrains.Annotations;
using ModSettings.Core;
using UnityEngine;

namespace ModSettings.Common {
  public class ReadonlyTextModSetting : NonPersistentSetting {

    public TextSettings Settings { get; }

    public ReadonlyTextModSetting(ModSettingDescriptor descriptor, 
                                  TextSettings settings) : base(descriptor) {
      Settings = settings;
    }

    public class TextSettings {

      public TextAnchor TextAlignment { get; }
      public int FontSize { get; }
      public Vector4 Margin { get; }
      
      [UsedImplicitly]
      public TextSettings(TextAnchor textAlignment = TextAnchor.MiddleLeft, 
                          int fontSize = 14, 
                          Vector4 margin = default) {
        TextAlignment = textAlignment;
        FontSize = fontSize;
        Margin = margin;
      }
      
    }

  }
}