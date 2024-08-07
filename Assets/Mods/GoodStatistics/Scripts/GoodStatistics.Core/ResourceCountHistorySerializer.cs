using Timberborn.Goods;
using Timberborn.Persistence;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Core {
  public class ResourceCountHistorySerializer : IObjectSerializer<ResourceCountHistory> {

    private static readonly PropertyKey<SavedGood> GoodKey = new("Good");
    private static readonly ListKey<ResourceCount> ResourceCountsKey = new("ResourceCounts");
    private readonly SavedGoodObjectSerializer _savedGoodObjectSerializer;
    private readonly ResourceCountSerializer _resourceCountSerializer;

    public ResourceCountHistorySerializer(SavedGoodObjectSerializer savedGoodObjectSerializer,
                                          ResourceCountSerializer resourceCountSerializer) {
      _savedGoodObjectSerializer = savedGoodObjectSerializer;
      _resourceCountSerializer = resourceCountSerializer;
    }

    public void Serialize(ResourceCountHistory value, IObjectSaver objectSaver) {
      objectSaver.Set(GoodKey, SavedGood.Create(value.GoodId), _savedGoodObjectSerializer);
      objectSaver.Set(ResourceCountsKey, value.ResourceCounts, _resourceCountSerializer);
    }

    public Obsoletable<ResourceCountHistory> Deserialize(IObjectLoader objectLoader) {
      var resourceCounts = objectLoader.Get(ResourceCountsKey, _resourceCountSerializer);
      return objectLoader.GetObsoletable(GoodKey, _savedGoodObjectSerializer, out var savedGood)
          ? ResourceCountHistory.CreateFromSave(savedGood.Id, resourceCounts)
          : default(Obsoletable<ResourceCountHistory>);
    }

  }
}