using GoodStatistics.Core;
using Timberborn.SingletonSystem;

namespace GoodStatistics.Analytics {
  internal class GlobalResourceTrendsRegistry : ILoadableSingleton {

    public GoodTrendsRegistry GoodTrendsRegistry { get; private set; }
    private readonly GlobalResourceCountsRegistry _globalResourceCountsRegistry;
    private readonly GoodTrendsRegistryFactory _goodTrendsRegistryFactory;

    public GlobalResourceTrendsRegistry(GlobalResourceCountsRegistry globalResourceCountsRegistry,
                                        GoodTrendsRegistryFactory goodTrendsRegistryFactory) {
      _globalResourceCountsRegistry = globalResourceCountsRegistry;
      _goodTrendsRegistryFactory = goodTrendsRegistryFactory;
    }

    public void Load() {
      GoodTrendsRegistry = _goodTrendsRegistryFactory.Create(
          _globalResourceCountsRegistry.ResourceCountsRegistry);
    }

  }
}