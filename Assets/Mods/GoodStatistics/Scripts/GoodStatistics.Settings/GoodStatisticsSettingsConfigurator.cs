using Bindito.Core;

namespace GoodStatistics.Settings {
  [Context("Game")]
  [Context("MainMenu")]
  public class GoodStatisticsSettingsConfigurator : Configurator {

    protected override void Configure() {
      Bind<GoodStatisticsSettings>().AsSingleton();
      Bind<EMAAnalyzerSettings>().AsSingleton();
    }

  }
}