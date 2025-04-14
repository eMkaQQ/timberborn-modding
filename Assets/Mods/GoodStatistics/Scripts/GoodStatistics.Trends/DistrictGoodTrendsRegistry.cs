using Bindito.Core;
using GoodStatistics.Sampling;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;

namespace GoodStatistics.Trends {
  public class DistrictGoodTrendsRegistry : BaseComponent,
                                            IInitializableEntity {

    public GoodTrendsRegistry GoodTrendsRegistry { get; private set; }
    private GoodTrendsRegistryFactory _goodTrendsRegistryFactory;

    [Inject]
    public void InjectDependencies(GoodTrendsRegistryFactory goodTrendsRegistryFactory) {
      _goodTrendsRegistryFactory = goodTrendsRegistryFactory;
    }

    public void InitializeEntity() {
      var goodSamplesRegistry =
          GetComponentFast<DistrictGoodSamplesRegistry>().GoodSamplesRegistry;
      GoodTrendsRegistry = _goodTrendsRegistryFactory.Create(goodSamplesRegistry);
    }

  }
}