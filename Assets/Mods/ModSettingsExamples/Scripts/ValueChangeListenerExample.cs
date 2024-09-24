using JetBrains.Annotations;
using Timberborn.Common;
using Timberborn.SingletonSystem;
using Timberborn.TitleScreenUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettingsExamples {
  internal class ValueChangeListenerExample : ILoadableSingleton {

    private readonly TitleScreenFooter _titleScreenFooter;
    [UsedImplicitly]
    private readonly TitleScreen _titleScreen;
    private readonly AdvancedSettingsExample _advancedSettingsExample;
    private VisualElement _mainMenuBackground;

    public ValueChangeListenerExample(TitleScreenFooter titleScreenFooter,
                                      TitleScreen titleScreen,
                                      AdvancedSettingsExample advancedSettingsExample) {
      _titleScreenFooter = titleScreenFooter;
      _titleScreen = titleScreen;
      _advancedSettingsExample = advancedSettingsExample;
    }

    public void Load() {
      var root = _titleScreenFooter.Root.parent;
      _mainMenuBackground = root.Q<VisualElement>("TitleScreen");
      Asserts.FieldIsNotNull(this, _mainMenuBackground, nameof(_mainMenuBackground));
      _advancedSettingsExample.BackgroundTintSetting.ValueChanged += OnBackgroundTintSettingChanged;
      ToggleBackgroundTint(_advancedSettingsExample.BackgroundTintSetting.Value);
    }

    private void OnBackgroundTintSettingChanged(object sender, bool e) {
      ToggleBackgroundTint(e);
    }

    private void ToggleBackgroundTint(bool value) {
      _mainMenuBackground.style.unityBackgroundImageTintColor = value ? Color.yellow : Color.white;
    }

  }
}