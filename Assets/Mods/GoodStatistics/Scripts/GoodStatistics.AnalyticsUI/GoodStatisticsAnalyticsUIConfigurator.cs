using Bindito.Core;

namespace GoodStatistics.AnalyticsUI {
  [Context("Game")]
  public class GoodStatisticsAnalyticsUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<TopBarTrendElementsUpdater>().AsSingleton();
      Bind<GoodTrendElementFactory>().AsSingleton();
      Bind<TrendIconProvider>().AsSingleton();
      Bind<TopBarPanelTrendDecorator>().AsSingleton();
      Bind<GoodTrendDaysLeftDescriber>().AsSingleton();
      Bind<GoodTrendTooltipFactory>().AsSingleton();
    }

  }
}