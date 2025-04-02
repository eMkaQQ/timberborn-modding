using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.Sampling {
  public class GoodSampleRecordsSerializer : IValueSerializer<GoodSampleRecords> {

    private static readonly PropertyKey<SavedGood> GoodKey = new("Good");
    private static readonly ListKey<GoodSample> GoodSamplesKey = new("GoodSamples");
    private readonly SavedGoodValueSerializer _savedGoodValueSerializer;
    private readonly GoodSampleSerializer _goodSampleSerializer;

    public GoodSampleRecordsSerializer(SavedGoodValueSerializer savedGoodValueSerializer,
                                       GoodSampleSerializer goodSampleSerializer) {
      _savedGoodValueSerializer = savedGoodValueSerializer;
      _goodSampleSerializer = goodSampleSerializer;
    }

    public void Serialize(GoodSampleRecords value, IValueSaver valueSaver) {
      var objectSaver = valueSaver.AsObject();
      objectSaver.Set(GoodKey, new(value.GoodId), _savedGoodValueSerializer);
      objectSaver.Set(GoodSamplesKey, value.GoodSamples, _goodSampleSerializer);
    }

    public Obsoletable<GoodSampleRecords> Deserialize(IValueLoader valueLoader) {
      var objectLoader = valueLoader.AsObject();
      var goodSamples = objectLoader.Get(GoodSamplesKey, _goodSampleSerializer);
      return objectLoader.GetObsoletable(GoodKey, _savedGoodValueSerializer, out var savedGood)
          ? GoodSampleRecords.CreateFromSave(savedGood.Id, goodSamples)
          : default(Obsoletable<GoodSampleRecords>);
    }

  }
}