using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModSettingsExamples {
  internal class SettingsLogger : ILoadableSingleton {

    private readonly AdvancedSettingsExample _advancedSettingsExample;
    private readonly SimpleSettingsExample _simpleSettingsExample;

    public SettingsLogger(AdvancedSettingsExample advancedSettingsExample,
                          SimpleSettingsExample simpleSettingsExample) {
      _advancedSettingsExample = advancedSettingsExample;
      _simpleSettingsExample = simpleSettingsExample;
    }

    public void Load() {
      Debug.Log($"{nameof(_advancedSettingsExample.SmallIntRangeSetting)} value:"
                + $" {_advancedSettingsExample.SmallIntRangeSetting.Value}");
      Debug.Log($"{nameof(_advancedSettingsExample.NegativeRangeSetting)} value:"
                + $" {_advancedSettingsExample.NegativeRangeSetting}");
      Debug.Log($"{nameof(_advancedSettingsExample.DropdownSetting)} value:"
                + $" {_advancedSettingsExample.DropdownSetting.Value}");
      Debug.Log($"{nameof(_simpleSettingsExample.IntSetting)} value:"
                + $" {_simpleSettingsExample.IntSetting.Value}");
      Debug.Log($"{nameof(_simpleSettingsExample.FloatSetting)} value:"
                + $" {_simpleSettingsExample.FloatSetting.Value}");
      Debug.Log($"{nameof(_simpleSettingsExample.StringSetting)} value:"
                + $" {_simpleSettingsExample.StringSetting.Value}");
      Debug.Log($"{nameof(_simpleSettingsExample.BoolSetting)} value:"
                + $" {_simpleSettingsExample.BoolSetting.Value}");
    }

  }
}