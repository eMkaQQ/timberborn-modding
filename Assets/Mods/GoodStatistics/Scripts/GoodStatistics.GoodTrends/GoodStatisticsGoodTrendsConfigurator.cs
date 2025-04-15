using Bindito.Core;
using GoodStatistics.GoodSampling;
using Timberborn.TemplateSystem;

namespace GoodStatistics.GoodTrends {
  [Context("Game")]
  public class GoodStatisticsGoodTrendsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
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