using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.GoodSampling {
  public class GoodSampleRecordsSerializer : IValueSerializer<GoodSampleRecords> {

    private static readonly PropertyKey<SerializedGood> GoodKey = new("Good");
    private static readonly ListKey<GoodSample> GoodSamplesKey = new("GoodSamples");
    private readonly SerializedGoodValueSerializer _serializedGoodValueSerializer;
    private readonly GoodSampleSerializer _goodSampleSerializer;

    public GoodSampleRecordsSerializer(SerializedGoodValueSerializer serializedGoodValueSerializer,
                                       GoodSampleSerializer goodSampleSerializer) {
      _serializedGoodValueSerializer = serializedGoodValueSerializer;
      _goodSampleSerializer = goodSampleSerializer;
    }

    public void Serialize(GoodSampleRecords value, IValueSaver valueSaver) {
      var objectSaver = valueSaver.AsObject();
      objectSaver.Set(GoodKey, new(value.GoodId), _serializedGoodValueSerializer);
      objectSaver.Set(GoodSamplesKey, value.GoodSamples, _goodSampleSerializer);
    }

    public Obsoletable<GoodSampleRecords> Deserialize(IValueLoader valueLoader) {
      var objectLoader = valueLoader.AsObject();
      var goodSamples = objectLoader.Get(GoodSamplesKey, _goodSampleSerializer);
      return objectLoader.GetObsoletable(GoodKey, _serializedGoodValueSerializer, out var savedGood)
          ? GoodSampleRecords.CreateFromSave(savedGood.Id, goodSamples)
          : default(Obsoletable<GoodSampleRecords>);
    }

  }
}