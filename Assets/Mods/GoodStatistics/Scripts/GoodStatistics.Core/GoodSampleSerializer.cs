using Timberborn.Persistence;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Core {
  public class GoodSampleSerializer : IObjectSerializer<GoodSample> {

    private static readonly PropertyKey<ResourceCount> ResourceCountKey = new("ResourceCount");
    private static readonly PropertyKey<float> DayTimestampKey = new("DayTimestamp");
    private readonly ResourceCountSerializer _resourceCountSerializer;

    public GoodSampleSerializer(ResourceCountSerializer resourceCountSerializer) {
      _resourceCountSerializer = resourceCountSerializer;
    }

    public void Serialize(GoodSample value, IObjectSaver objectSaver) {
      objectSaver.Set(ResourceCountKey, value.ResourceCount, _resourceCountSerializer);
      objectSaver.Set(DayTimestampKey, value.DayTimestamp);
    }

    public Obsoletable<GoodSample> Deserialize(IObjectLoader objectLoader) {
      var resourceCount = objectLoader.Get(ResourceCountKey, _resourceCountSerializer);
      var dayTimestamp = objectLoader.Get(DayTimestampKey);
      return new(new(resourceCount, dayTimestamp));
    }

  }
}