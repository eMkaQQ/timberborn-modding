using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.Sampling {
  public class GoodSampleRecordsSerializer : IObjectSerializer<GoodSampleRecords> {

    private static readonly PropertyKey<SavedGood> GoodKey = new("Good");
    private static readonly ListKey<GoodSample> GoodSamplesKey = new("GoodSamples");
    private readonly SavedGoodObjectSerializer _savedGoodObjectSerializer;
    private readonly GoodSampleSerializer _goodSampleSerializer;

    public GoodSampleRecordsSerializer(SavedGoodObjectSerializer savedGoodObjectSerializer,
                                       GoodSampleSerializer goodSampleSerializer) {
      _savedGoodObjectSerializer = savedGoodObjectSerializer;
      _goodSampleSerializer = goodSampleSerializer;
    }

    public void Serialize(GoodSampleRecords value, IObjectSaver objectSaver) {
      objectSaver.Set(GoodKey, SavedGood.Create(value.GoodId), _savedGoodObjectSerializer);
      objectSaver.Set(GoodSamplesKey, value.GoodSamples, _goodSampleSerializer);
    }

    public Obsoletable<GoodSampleRecords> Deserialize(IObjectLoader objectLoader) {
      var goodSamples = objectLoader.Get(GoodSamplesKey, _goodSampleSerializer);
      return objectLoader.GetObsoletable(GoodKey, _savedGoodObjectSerializer, out var savedGood)
          ? GoodSampleRecords.CreateFromSave(savedGood.Id, goodSamples)
          : default(Obsoletable<GoodSampleRecords>);
    }

  }
}