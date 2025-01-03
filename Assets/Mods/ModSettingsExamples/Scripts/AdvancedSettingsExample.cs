using JetBrains.Annotations;
using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;
using UnityEngine;

namespace ModSettingsExamples {
  internal class AdvancedSettingsExample : ModSettingsOwner {

    public ModSetting<int> SmallIntRangeSetting { get; private set; }

    public ModSetting<bool> SliderDisablerSetting { get; } = new(
        false, ModSettingDescriptor.Create("Disable slider above"));

    public ModSetting<bool> BackgroundTintSetting { get; } = new(
        false, ModSettingDescriptor.Create("Tint background color"));

    public ColorModSetting ColorSetting { get; } = new(
        new Color(0, 1, 1), ModSettingDescriptor.Create("Color setting"), false);

    public ColorModSetting TransparentColorSetting { get; } = new(
        "FF00FF88", ModSettingDescriptor.Create("Transparent color"), true);

    public LimitedStringModSetting DropdownSetting { get; } = new(
        0, new[] {
            new LimitedStringModSettingValue("value1", "eMka.ModSettingsExamples.Dropdown1"),
            new LimitedStringModSettingValue("value2", "eMka.ModSettingsExamples.Dropdown2"),
            new LimitedStringModSettingValue("value3", "eMka.ModSettingsExamples.Dropdown3")
        }, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.Dropdown"));

    public LimitedStringModSetting NonLocalizedDropdownSetting { get; } =
      new(0, new[] {
          new NonLocalizedLimitedStringModSettingValue("Custom value1"),
          new NonLocalizedLimitedStringModSettingValue("Custom value2"),
          new NonLocalizedLimitedStringModSettingValue("Custom value3")
      }, ModSettingDescriptor.Create("Non-localized dropdown"));

    public ModSetting<bool> LogValuesSetting { get; } = new(
        true, ModSettingDescriptor.Create("Log all these values"));

    [UsedImplicitly]
    public DummyButton DummyButton { get; } = new(ModSettingDescriptor.Create("Dummy button"));

    private RangeIntModSetting _negativeRangeSetting;

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
      _negativeRangeSetting = new(50, -100, 100,
                                  ModSettingDescriptor.Create("Custom label")
                                      .SetTooltip("With custom tooltip"));
      AddCustomModSetting(_negativeRangeSetting, "NegativeRangeSetting");
      SmallIntRangeSetting = new RangeIntModSetting(
          5, 0, 10, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.SmallIntRange")
              .SetEnableCondition(() => !SliderDisablerSetting.Value));
    }

  }
}