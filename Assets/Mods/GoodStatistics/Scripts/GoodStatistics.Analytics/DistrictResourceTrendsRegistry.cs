using Bindito.Core;
using GoodStatistics.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;

namespace GoodStatistics.Analytics {
  internal class DistrictResourceTrendsRegistry : BaseComponent,
                                                  IInitializableEntity {

    public GoodTrendsRegistry GoodTrendsRegistry { get; private set; }
    private GoodTrendsRegistryFactory _goodTrendsRegistryFactory;

    [Inject]
    public void InjectDependencies(GoodTrendsRegistryFactory goodTrendsRegistryFactory) {
      _goodTrendsRegistryFactory = goodTrendsRegistryFactory;
    }

    public void InitializeEntity() {
      var resourceCountsRegistry =
          GetComponentFast<DistrictResourceCountsRegistry>().ResourceCountsRegistry;
      GoodTrendsRegistry = _goodTrendsRegistryFactory.Create(resourceCountsRegistry);
    }

  }
}