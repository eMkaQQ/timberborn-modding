using Bindito.Core;
using Timberborn.GameDistricts;
using Timberborn.TemplateInstantiation;

namespace GoodStatistics.Sampling {
  [Context("Game")]
  public class GoodStatisticsSamplingConfigurator : Configurator {

    protected override void Configure() {
      Bind<DistrictGoodSamplesRegistry>().AsTransient();
      
      Bind<GlobalGoodSamplesRegistry>().AsSingleton();
      Bind<GoodsSampler>().AsSingleton();
      Bind<GoodsSampleTrigger>().AsSingleton();
      Bind<ResourceCountSerializer>().AsSingleton();
      Bind<GoodSampleSerializer>().AsSingleton();
      Bind<GoodSampleRecordsSerializer>().AsSingleton();
      Bind<GoodSamplesRegistrySerializer>().AsSingleton();
      Bind<SampleTimeCalculator>().AsSingleton();
      MultiBind<TemplateModule>()
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