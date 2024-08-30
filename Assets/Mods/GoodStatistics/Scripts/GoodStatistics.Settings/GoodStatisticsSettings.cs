using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace GoodStatistics.Settings {
  public class GoodStatisticsSettings : ModSettingsOwner {

    public ModSetting<int> SamplesPerDay { get; } =
      new RangeIntModSetting(
          3, 1, 12,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.Settings.SamplesPerDay"));

    public ModSetting<bool> ShowTrendsOnTopBar { get; } =
      new(true,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.Settings.ShowTrendsOnTopBar"));

    public ModSetting<bool> AnalyzeGoodsWithoutStockpiles { get; } =
      new(false, ModSettingDescriptor.CreateLocalized(
              "eMka.GoodStatistics.Settings.AnalyzeGoodsWithoutStockpiles"));

    public ModSetting<int> IgnoreGoodsWithStorageLessThan { get; } =
      new(50, ModSettingDescriptor.CreateLocalized(
              "eMka.GoodStatistics.Settings.IgnoreGoodsWithStorageLessThan"));

    public GoodStatisticsSettings(ISettings settings,
                                  ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                  ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.GoodStatistics.Settings.BaseSettings";

    protected override string ModId => "eMka.GoodsStatistics";

  }
}