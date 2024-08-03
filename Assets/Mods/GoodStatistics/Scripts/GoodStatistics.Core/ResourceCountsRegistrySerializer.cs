using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.Core {
  internal class ResourceCountsRegistrySerializer : IObjectSerializer<ResourceCountsRegistry> {

    private static readonly ListKey<ResourceCountHistory> ResourceCountHistoriesKey =
        new("ResourceCountHistories");
    private readonly ResourceCountHistorySerializer _resourceCountHistorySerializer;
    private readonly IGoodService _goodService;

    public ResourceCountsRegistrySerializer(ResourceCountHistorySerializer
                                                resourceCountHistorySerializer,
                                            IGoodService goodService) {
      _resourceCountHistorySerializer = resourceCountHistorySerializer;
      _goodService = goodService;
    }

    public void Serialize(ResourceCountsRegistry value, IObjectSaver objectSaver) {
      objectSaver.Set(ResourceCountHistoriesKey, value.ResourceCountHistories,
                      _resourceCountHistorySerializer);
    }

    public Obsoletable<ResourceCountsRegistry> Deserialize(IObjectLoader objectLoader) {
      var resourceCountsRegistry = ResourceCountsRegistry.CreateFromSave(
          objectLoader.Get(ResourceCountHistoriesKey, _resourceCountHistorySerializer));
      foreach (var goodId in _goodService.Goods) {
        if (!resourceCountsRegistry.HasGood(goodId)) {
          resourceCountsRegistry.AddMissingGood(goodId);
        }
      }
      return new(resourceCountsRegistry);
    }

  }
}