using Bindito.Core;
using Timberborn.GameDistricts;
using Timberborn.TemplateSystem;

namespace GoodStatistics.Core {
  [Context("Game")]
  public class GoodStatisticsCoreConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GlobalResourceCountsRegistry>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsSampler>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsSampleTrigger>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsSettings>().AsSingleton();
      containerDefinition.Bind<ResourceCountSerializer>().AsSingleton();
      containerDefinition.Bind<ResourceCountHistorySerializer>().AsSingleton();
      containerDefinition.Bind<ResourceCountsRegistrySerializer>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>()
          .ToProvider(ProvideTemplateModule)
          .AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<DistrictCenter, DistrictResourceCountsRegistry>();
      return builder.Build();
    }

  }
}