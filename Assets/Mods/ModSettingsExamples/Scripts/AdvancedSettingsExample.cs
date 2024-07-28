using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace ModSettingsExamples {
  internal class AdvancedSettingsExample : ModSettingsOwner {

    public ModSetting<int> SmallIntRangeSetting { get; } = new RangeIntModSetting(
        5, 0, 10, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.SmallIntRange"));

    public ModSetting<string> DropdownSetting { get; } = new LimitedStringModSetting(
        0, new[] {
            new LimitedStringModSettingValue("value1", "eMka.ModSettingsExamples.Dropdown1"),
            new LimitedStringModSettingValue("value2", "eMka.ModSettingsExamples.Dropdown2"),
            new LimitedStringModSettingValue("value3", "eMka.ModSettingsExamples.Dropdown3")
        }, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.Dropdown"));

    public ModSetting<string> NonLocalizedDropdownSetting { get; } =
      new LimitedStringModSetting(0, new[] {
          new NonLocalizedLimitedStringModSettingValue("Custom value1"),
          new NonLocalizedLimitedStringModSettingValue("Custom value2"),
          new NonLocalizedLimitedStringModSettingValue("Custom value3")
      }, ModSettingDescriptor.Create("Non-localized dropdown"));

    private ModSetting<int> _negativeRangeSetting;

    public AdvancedSettingsExample(ISettings settings,
                                   ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                   ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.ModSettingsExamples.AdvancedSettingsHeader";

    public override int Order => 10;

    public int NegativeRangeSetting => _negativeRangeSetting.Value;

    protected override string ModId => "eMka.ModSettingsExamples";

    protected override void OnBeforeLoad() {
      _negativeRangeSetting = new RangeIntModSetting(50, -100, 100,
                                                     ModSettingDescriptor.Create("Custom label")
                                                         .SetTooltip("With custom tooltip"));
      AddCustomModSetting(_negativeRangeSetting, "NegativeRangeSetting");
    }

  }
}