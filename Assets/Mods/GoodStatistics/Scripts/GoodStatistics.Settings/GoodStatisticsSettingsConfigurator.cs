using Bindito.Core;

namespace GoodStatistics.Settings {
  [Context("Game")]
  [Context("MainMenu")]
  public class GoodStatisticsSettingsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GoodStatisticsSettings>().AsSingleton();
    }

  }
}