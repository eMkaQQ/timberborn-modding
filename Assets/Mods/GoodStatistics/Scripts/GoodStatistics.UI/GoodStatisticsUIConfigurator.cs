using Bindito.Core;

namespace GoodStatistics.UI {
  [Context("Game")]
  public class GoodStatisticsUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<GoodSampleRecordsElementFactory>().AsSingleton();
    }

  }
}