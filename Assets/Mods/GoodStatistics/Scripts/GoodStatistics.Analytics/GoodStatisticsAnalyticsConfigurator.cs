using Bindito.Core;

namespace GoodStatistics.Analytics {
  [Context("Game")]
  public class GoodStatisticsAnalyticsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ITrendAnalyzer>().To<EMAAnalyzer>().AsSingleton();
    }

  }
}