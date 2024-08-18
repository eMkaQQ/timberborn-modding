using Bindito.Core;

namespace GoodStatistics.UI {
  [Context("Game")]
  public class GoodStatisticsUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GoodSampleRecordsElementFactory>().AsSingleton();
    }

  }
}