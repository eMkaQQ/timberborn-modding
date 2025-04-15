using Bindito.Core;

namespace GoodStatistics.GoodTrendsUI {
  [Context("Game")]
  public class GoodStatisticsGoodTrendsUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<TopBarTrendElementsUpdater>().AsSingleton();
      containerDefinition.Bind<GoodTrendElementFactory>().AsSingleton();
      containerDefinition.Bind<TrendIconProvider>().AsSingleton();
      containerDefinition.Bind<TopBarPanelTrendDecorator>().AsSingleton();
      containerDefinition.Bind<GoodTrendDaysLeftDescriber>().AsSingleton();
      containerDefinition.Bind<GoodTrendTooltipFactory>().AsSingleton();
    }

  }
}