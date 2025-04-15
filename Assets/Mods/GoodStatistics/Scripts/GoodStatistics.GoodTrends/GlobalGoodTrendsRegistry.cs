using GoodStatistics.GoodSampling;
using Timberborn.SingletonSystem;

namespace GoodStatistics.GoodTrends {
  public class GlobalGoodTrendsRegistry : ILoadableSingleton {

    public GoodTrendsRegistry GoodTrendsRegistry { get; private set; }
    private readonly GlobalGoodSamplesRegistry _globalGoodSamplesRegistry;
    private readonly GoodTrendsRegistryFactory _goodTrendsRegistryFactory;

    public GlobalGoodTrendsRegistry(GlobalGoodSamplesRegistry globalGoodSamplesRegistry,
                                    GoodTrendsRegistryFactory goodTrendsRegistryFactory) {
      _globalGoodSamplesRegistry = globalGoodSamplesRegistry;
      _goodTrendsRegistryFactory = goodTrendsRegistryFactory;
    }

    public void Load() {
      GoodTrendsRegistry = _goodTrendsRegistryFactory.Create(
          _globalGoodSamplesRegistry.GoodSamplesRegistry);
    }

  }
}