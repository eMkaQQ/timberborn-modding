using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace GoodStatistics.Settings {
  public class EMAAnalyzerSettings : ModSettingsOwner {

    public ModSetting<int> SamplesToAnalyze { get; } =
      new RangeIntModSetting(
          10, 5, GoodStatisticsConstants.MaxSamples,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.Settings.SamplesToAnalyze"));

    public ModSetting<int> LowChangeThreshold { get; } =
      new RangeIntModSetting(
          1, 1, 100,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.Settings.SlowChangeThreshold"));

    public ModSetting<int> MediumChangeThreshold { get; } =
      new RangeIntModSetting(
          5, 1, 100,
          ModSettingDescriptor.CreateLocalized(
              "eMka.GoodStatistics.Settings.ModerateChangeThreshold"));

    public ModSetting<int> HighChangeThreshold { get; } =
      new RangeIntModSetting(
          10, 1, 100,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.Settings.FastChangeThreshold"));

    public ModSetting<int> ConsecutiveMicroChangesThreshold { get; } =
      new RangeIntModSetting(
          5, 1, GoodStatisticsConstants.MaxSamples,
          ModSettingDescriptor.CreateLocalized(
              "eMka.GoodStatistics.Settings.ConsecutiveMicroChangesThreshold"));

    public EMAAnalyzerSettings(ISettings settings,
                               ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                               ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override int Order => 5;
    public override string HeaderLocKey => "eMka.GoodStatistics.Settings.EMAAnalyzerHeader";
    protected override string ModId => "eMka.GoodsStatistics";

  }
}