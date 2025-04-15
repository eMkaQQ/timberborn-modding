using Bindito.Core;

namespace GoodStatistics.GoodUI {
  [Context("Game")]
  public class GoodStatisticsGoodUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GoodSampleRecordsElementFactory>().AsSingleton();
    }

  }
}