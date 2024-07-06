using ModSettings;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace ModSettingsExamples {
  internal class AdvancedSettingsExample : ModSettingsOwner {

    public ModSetting<int> SmallIntRangeSetting { get; } = new RangeIntModSetting(
        "eMka.ModSettingsExamples.SmallIntRange", 5, 0, 10);

    public ModSetting<int> NegativeRangeSetting { get; } = new RangeIntModSetting(
        "eMka.ModSettingsExamples.BigIntRange", -50, -100, 100);

    public ModSetting<string> DropdownSetting { get; } = new LimitedStringModSetting(
        "eMka.ModSettingsExamples.Dropdown", 0, new[] {
            new LimitedStringModSettingValue("value1", "eMka.ModSettingsExamples.Dropdown1"),
            new LimitedStringModSettingValue("value2", "eMka.ModSettingsExamples.Dropdown2"),
            new LimitedStringModSettingValue("value3", "eMka.ModSettingsExamples.Dropdown3")
        });

    public AdvancedSettingsExample(ISettings settings,
                                   ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                   ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.ModSettingsExamples.AdvancedSettingsHeader";

    public override int Order => 10;
    
    protected override string ModId => "eMka.ModSettingsExamples";

  }
}