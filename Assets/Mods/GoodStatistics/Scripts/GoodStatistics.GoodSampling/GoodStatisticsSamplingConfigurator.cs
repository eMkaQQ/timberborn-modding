using Bindito.Core;
using Timberborn.GameDistricts;
using Timberborn.TemplateSystem;

namespace GoodStatistics.GoodSampling {
  [Context("Game")]
  public class GoodStatisticsSamplingConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GlobalGoodSamplesRegistry>().AsSingleton();
      containerDefinition.Bind<GoodsSampler>().AsSingleton();
      containerDefinition.Bind<GoodsSampleTrigger>().AsSingleton();
      containerDefinition.Bind<ResourceCountSerializer>().AsSingleton();
      containerDefinition.Bind<GoodSampleSerializer>().AsSingleton();
      containerDefinition.Bind<GoodSampleRecordsSerializer>().AsSingleton();
      containerDefinition.Bind<GoodSamplesRegistrySerializer>().AsSingleton();
      containerDefinition.Bind<SampleTimeCalculator>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>()
          .ToProvider(ProvideTemplateModule)
          .AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<DistrictCenter, DistrictGoodSamplesRegistry>();
      return builder.Build();
    }

  }
}