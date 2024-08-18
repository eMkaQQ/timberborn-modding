using Timberborn.Goods;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace GoodStatistics.Sampling {
  public class GlobalGoodSamplesRegistry : ISaveableSingleton,
                                           ILoadableSingleton {

    private static readonly SingletonKey GlobalGoodSamplesRegistryKey =
        new("GlobalGoodSamplesRegistry");
    private static readonly PropertyKey<GoodSamplesRegistry> GoodSamplesRegistryKey =
        new("GoodSamplesRegistry");
    public GoodSamplesRegistry GoodSamplesRegistry { get; private set; }
    private readonly IGoodService _goodService;
    private readonly ISingletonLoader _singletonLoader;
    private readonly GoodSamplesRegistrySerializer _goodSamplesRegistrySerializer;

    public GlobalGoodSamplesRegistry(IGoodService goodService,
                                     ISingletonLoader singletonLoader,
                                     GoodSamplesRegistrySerializer
                                         goodSamplesRegistrySerializer) {
      _goodService = goodService;
      _singletonLoader = singletonLoader;
      _goodSamplesRegistrySerializer = goodSamplesRegistrySerializer;
    }

    public void Save(ISingletonSaver singletonSaver) {
      var globalGoodSamplesRegistry = singletonSaver.GetSingleton(GlobalGoodSamplesRegistryKey);
      globalGoodSamplesRegistry.Set(GoodSamplesRegistryKey, GoodSamplesRegistry,
                                    _goodSamplesRegistrySerializer);
    }

    public void Load() {
      if (_singletonLoader.HasSingleton(GlobalGoodSamplesRegistryKey)) {
        var globalGoodSamplesRegistry =
            _singletonLoader.GetSingleton(GlobalGoodSamplesRegistryKey);
        GoodSamplesRegistry = globalGoodSamplesRegistry.Get(GoodSamplesRegistryKey,
                                                            _goodSamplesRegistrySerializer);
      } else {
        GoodSamplesRegistry = GoodSamplesRegistry.CreateNew(_goodService.Goods);
      }
    }

  }
}