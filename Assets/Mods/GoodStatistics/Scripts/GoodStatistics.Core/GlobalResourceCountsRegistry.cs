using Timberborn.Goods;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace GoodStatistics.Core {
  public class GlobalResourceCountsRegistry : ISaveableSingleton,
                                              ILoadableSingleton {

    private static readonly SingletonKey GlobalResourceCountsRegistryKey =
        new("GlobalResourceCountsRegistry");
    private static readonly PropertyKey<ResourceCountsRegistry> ResourceCountsRegistryKey =
        new("ResourceCountsRegistry");
    public ResourceCountsRegistry ResourceCountsRegistry { get; private set; }
    private readonly IGoodService _goodService;
    private readonly ISingletonLoader _singletonLoader;
    private readonly ResourceCountsRegistrySerializer _resourceCountsRegistrySerializer;

    public GlobalResourceCountsRegistry(IGoodService goodService,
                                        ISingletonLoader singletonLoader,
                                        ResourceCountsRegistrySerializer
                                            resourceCountsRegistrySerializer) {
      _goodService = goodService;
      _singletonLoader = singletonLoader;
      _resourceCountsRegistrySerializer = resourceCountsRegistrySerializer;
    }

    public void Save(ISingletonSaver singletonSaver) {
      var globalResourceCountsRegistry =
          singletonSaver.GetSingleton(GlobalResourceCountsRegistryKey);
      globalResourceCountsRegistry.Set(ResourceCountsRegistryKey, ResourceCountsRegistry,
                                       _resourceCountsRegistrySerializer);
    }

    public void Load() {
      if (_singletonLoader.HasSingleton(GlobalResourceCountsRegistryKey)) {
        var globalResourceCountsRegistry =
            _singletonLoader.GetSingleton(GlobalResourceCountsRegistryKey);
        ResourceCountsRegistry = globalResourceCountsRegistry.Get(
            ResourceCountsRegistryKey, _resourceCountsRegistrySerializer);
      } else {
        ResourceCountsRegistry = ResourceCountsRegistry.CreateNew(_goodService.Goods);
      }
    }

  }
}