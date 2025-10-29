using Bindito.Core;
using GoodStatistics.Sampling;
using Timberborn.TemplateInstantiation;

namespace GoodStatistics.Analytics {
  [Context("Game")]
  public class GoodStatisticsAnalyticsConfigurator : Configurator {

    protected override void Configure() {
      Bind<DistrictGoodTrendsRegistry>().AsTransient();
      
      Bind<IGoodTrendAnalyzer>().To<EMAAnalyzer>().AsSingleton();
      Bind<GlobalGoodTrendsRegistry>().AsSingleton();
      Bind<GoodTrendsRegistryFactory>().AsSingleton();
      MultiBind<TemplateModule>()
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