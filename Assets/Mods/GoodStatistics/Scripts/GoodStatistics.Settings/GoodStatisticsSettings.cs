using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace GoodStatistics.Settings {
  public class GoodStatisticsSettings : ModSettingsOwner {

    public ModSetting<int> SamplesPerDay { get; } =
      new RangeIntModSetting(
          1, 1, 48,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.SamplePerDaySetting"));

    public GoodStatisticsSettings(ISettings settings,
                                  ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                  ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.GoodStatistics.SamplingSettings";

    protected override string ModId => "eMka.GoodsStatistics";

  }
}