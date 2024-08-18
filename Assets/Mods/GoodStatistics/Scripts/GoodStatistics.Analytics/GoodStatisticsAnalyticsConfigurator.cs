using Bindito.Core;
using GoodStatistics.Sampling;
using Timberborn.TemplateSystem;

namespace GoodStatistics.Analytics {
  [Context("Game")]
  public class GoodStatisticsAnalyticsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<IGoodTrendAnalyzer>().To<EMAAnalyzer>().AsSingleton();
      containerDefinition.Bind<GlobalGoodTrendsRegistry>().AsSingleton();
      containerDefinition.Bind<GoodTrendsRegistryFactory>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>()
          .ToProvider(ProvideTemplateModule)
          .AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<DistrictGoodSamplesRegistry, DistrictGoodTrendsRegistry>();
      return builder.Build();
    }

  }
}