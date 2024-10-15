using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace ModSettingsExamples {
  internal class AdvancedSettingsExample : ModSettingsOwner {

    public ModSetting<int> SmallIntRangeSetting { get; private set; }

    public ModSetting<bool> SliderDisablerSetting { get; } = new(
        false, ModSettingDescriptor.Create("Disable slider above"));

    public ModSetting<bool> BackgroundTintSetting { get; } = new(
        false, ModSettingDescriptor.Create("Tint background color"));

    public ModSetting<string> ColorSetting { get; } = new ColorModSetting(
        "00FFFF", ModSettingDescriptor.Create("Color setting"), false);

    public ModSetting<string> TransparentColorSetting { get; } = new ColorModSetting(
        "FF00FF88", ModSettingDescriptor.Create("Transparent color"), true);

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

    public ModSetting<bool> LogValuesSetting { get; } = new(
        true, ModSettingDescriptor.Create("Log these values"));

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
      SmallIntRangeSetting = new RangeIntModSetting(
          5, 0, 10, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.SmallIntRange")
              .SetEnableCondition(() => !SliderDisablerSetting.Value));
    }

  }
}