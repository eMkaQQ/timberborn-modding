using Timberborn.Persistence;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.GoodSampling {
  public class GoodSampleSerializer : IValueSerializer<GoodSample> {

    private static readonly PropertyKey<ResourceCount> ResourceCountKey = new("ResourceCount");
    private static readonly PropertyKey<float> DayTimestampKey = new("DayTimestamp");
    private readonly ResourceCountSerializer _resourceCountSerializer;

    public GoodSampleSerializer(ResourceCountSerializer resourceCountSerializer) {
      _resourceCountSerializer = resourceCountSerializer;
    }

    public void Serialize(GoodSample value, IValueSaver valueSaver) {
      var objectSaver = valueSaver.AsObject();
      objectSaver.Set(ResourceCountKey, value.ResourceCount, _resourceCountSerializer);
      objectSaver.Set(DayTimestampKey, value.DayTimestamp);
    }

    public Obsoletable<GoodSample> Deserialize(IValueLoader valueLoader) {
      var objectLoader = valueLoader.AsObject();
      var resourceCount = objectLoader.Get(ResourceCountKey, _resourceCountSerializer);
      var dayTimestamp = objectLoader.Get(DayTimestampKey);
      return new(new(resourceCount, dayTimestamp));
    }

  }
}