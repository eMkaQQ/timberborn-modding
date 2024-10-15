using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModSettingsExamples {
  internal class SettingsLogger : ILoadableSingleton {

    private readonly AdvancedSettingsExample _advancedSettingsExample;
    private readonly SimpleSettingsExample _simpleSettingsExample;
    private readonly FileStoredSettingsExample _fileStoredSettingsExample;

    public SettingsLogger(AdvancedSettingsExample advancedSettingsExample,
                          SimpleSettingsExample simpleSettingsExample,
                          FileStoredSettingsExample fileStoredSettingsExample) {
      _advancedSettingsExample = advancedSettingsExample;
      _simpleSettingsExample = simpleSettingsExample;
      _fileStoredSettingsExample = fileStoredSettingsExample;
    }

    public void Load() {
      if (_advancedSettingsExample.LogValuesSetting.Value) {
        Debug.Log($"{nameof(_advancedSettingsExample.NegativeRangeSetting)} value:"
                  + $" {_advancedSettingsExample.NegativeRangeSetting}");
        Debug.Log($"{nameof(_advancedSettingsExample.SmallIntRangeSetting)} value:"
                  + $" {_advancedSettingsExample.SmallIntRangeSetting.Value}");
        Debug.Log($"{nameof(_advancedSettingsExample.SliderDisablerSetting)} value:"
                  + $" {_advancedSettingsExample.SliderDisablerSetting.Value}");
        Debug.Log($"{nameof(_advancedSettingsExample.ColorSetting)} value:"
                  + $" {_advancedSettingsExample.ColorSetting.Value}");
        Debug.Log($"{nameof(_advancedSettingsExample.TransparentColorSetting)} value:"
                  + $" {_advancedSettingsExample.TransparentColorSetting.Value}");
        Debug.Log($"{nameof(_advancedSettingsExample.DropdownSetting)} value:"
                  + $" {_advancedSettingsExample.DropdownSetting.Value}");
        Debug.Log($"{nameof(_advancedSettingsExample.NonLocalizedDropdownSetting)} value:"
                  + $" {_advancedSettingsExample.NonLocalizedDropdownSetting.Value}");
        Debug.Log($"{nameof(_simpleSettingsExample.IntSetting)} value:"
                  + $" {_simpleSettingsExample.IntSetting.Value}");
        Debug.Log($"{nameof(_simpleSettingsExample.FloatSetting)} value:"
                  + $" {_simpleSettingsExample.FloatSetting.Value}");
        Debug.Log($"{nameof(_simpleSettingsExample.StringSetting)} value:"
                  + $" {_simpleSettingsExample.StringSetting.Value}");
        Debug.Log($"{nameof(_simpleSettingsExample.BoolSetting)} value:"
                  + $" {_simpleSettingsExample.BoolSetting.Value}");
        Debug.Log($"{nameof(_fileStoredSettingsExample.LongStringSetting)} value:"
                  + $" {_fileStoredSettingsExample.LongStringSetting.Value}");
      }
    }

  }
}