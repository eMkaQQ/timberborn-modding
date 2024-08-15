using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace GoodStatistics.Settings {
  public class EMAAnalyzerSettings : ModSettingsOwner {

    public ModSetting<int> ConsecutiveMicroChangesThreshold { get; } =
      new RangeIntModSetting(
          4, 1, GoodStatisticsConstants.MaxSamples,
          ModSettingDescriptor.CreateLocalized(
              "eMka.GoodStatistics.ConsecutiveMicroChangesThreshold"));

    public ModSetting<int> LowChangeThreshold { get; } =
      new RangeIntModSetting(
          1, 1, 100,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.LowChangeThreshold"));

    public ModSetting<int> MediumChangeThreshold { get; } =
      new RangeIntModSetting(
          5, 1, 100,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.MediumChangeThreshold"));

    public ModSetting<int> HighChangeThreshold { get; } =
      new RangeIntModSetting(
          10, 1, 100,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.HighChangeThreshold"));

    public ModSetting<int> SamplesToAnalyze { get; } =
      new RangeIntModSetting(
          10, 5, GoodStatisticsConstants.MaxSamples,
          ModSettingDescriptor.CreateLocalized("eMka.GoodStatistics.SamplesToAnalyze"));

    public EMAAnalyzerSettings(ISettings settings,
                               ModSettingsOwnerRegistry
                                   modSettingsOwnerRegistry,
                               ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override int Order => 5;
    public override string HeaderLocKey => "eMka.GoodStatistics.EMAAnalyzerSettings";

    protected override string ModId => "eMka.GoodsStatistics";

    protected override void OnAfterLoad() {
      LowChangeThreshold.ValueChanged += (_, i) => {
        if (i > MediumChangeThreshold.Value) {
          MediumChangeThreshold.SetValue(i);
        }
      };
      MediumChangeThreshold.ValueChanged += (_, i) => {
        if (i < LowChangeThreshold.Value) {
          LowChangeThreshold.SetValue(i);
        }
        if (i > HighChangeThreshold.Value) {
          HighChangeThreshold.SetValue(i);
        }
      };
      HighChangeThreshold.ValueChanged += (_, i) => {
        if (i < MediumChangeThreshold.Value) {
          MediumChangeThreshold.SetValue(i);
        }
      };
    }

  }
}