using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.Core {
  public class ResourceCountHistorySerializer : IObjectSerializer<ResourceCountHistory> {

    private static readonly PropertyKey<SavedGood> GoodKey = new("Good");
    private static readonly ListKey<GoodSample> GoodSamplesKey = new("GoodSamples");
    private readonly SavedGoodObjectSerializer _savedGoodObjectSerializer;
    private readonly GoodSampleSerializer _goodSampleSerializer;

    public ResourceCountHistorySerializer(SavedGoodObjectSerializer savedGoodObjectSerializer,
                                          GoodSampleSerializer goodSampleSerializer) {
      _savedGoodObjectSerializer = savedGoodObjectSerializer;
      _goodSampleSerializer = goodSampleSerializer;
    }

    public void Serialize(ResourceCountHistory value, IObjectSaver objectSaver) {
      objectSaver.Set(GoodKey, SavedGood.Create(value.GoodId), _savedGoodObjectSerializer);
      objectSaver.Set(GoodSamplesKey, value.GoodSamples, _goodSampleSerializer);
    }

    public Obsoletable<ResourceCountHistory> Deserialize(IObjectLoader objectLoader) {
      var goodSamples = objectLoader.Get(GoodSamplesKey, _goodSampleSerializer);
      return objectLoader.GetObsoletable(GoodKey, _savedGoodObjectSerializer, out var savedGood)
          ? ResourceCountHistory.CreateFromSave(savedGood.Id, goodSamples)
          : default(Obsoletable<ResourceCountHistory>);
    }

  }
}