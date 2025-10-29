using Timberborn.Persistence;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Sampling {
  public class ResourceCountSerializer : IValueSerializer<ResourceCount> {

    private static readonly PropertyKey<int> InputOutputStockKey = new("InputOutputStock");
    private static readonly PropertyKey<int> OutputStockKey = new("OutputStock");
    private static readonly PropertyKey<int> InputOutputCapacityKey = new("InputOutputCapacity");
    private static readonly PropertyKey<float> FillRateKey = new("FillRate");

    public void Serialize(ResourceCount value, IValueSaver valueSaver) {
      var objectSaver = valueSaver.AsObject();
      objectSaver.Set(InputOutputStockKey, value.InputOutputStock);
      objectSaver.Set(OutputStockKey, value.OutputStock);
      objectSaver.Set(InputOutputCapacityKey, value.InputOutputCapacity);
      objectSaver.Set(FillRateKey, value.FillRate);
    }

    public Obsoletable<ResourceCount> Deserialize(IValueLoader valueLoader) {
      var objectLoader = valueLoader.AsObject();
      return new ResourceCount(objectLoader.Get(InputOutputStockKey),
                               objectLoader.Get(OutputStockKey),
                               objectLoader.Get(InputOutputCapacityKey),
                               objectLoader.Get(FillRateKey), 0);
    }

  }
}