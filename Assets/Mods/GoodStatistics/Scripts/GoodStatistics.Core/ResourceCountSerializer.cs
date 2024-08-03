using System.Linq;
using System.Reflection;
using Timberborn.Persistence;
using Timberborn.ResourceCountingSystem;
using Timberborn.SingletonSystem;

namespace GoodStatistics.Core {
  internal class ResourceCountSerializer : IObjectSerializer<ResourceCount>,
                                           ILoadableSingleton {

    private static readonly PropertyKey<int> InputOutputStockKey = new("InputOutputStock");
    private static readonly PropertyKey<int> OutputStockKey = new("OutputStock");
    private static readonly PropertyKey<int> InputOutputCapacityKey = new("InputOutputCapacity");
    private static readonly PropertyKey<float> FillRateKey = new("FillRate");
    private ConstructorInfo _constructor;

    public void Load() {
      _constructor = typeof(ResourceCount).GetConstructors(BindingFlags.NonPublic).Single();
    }

    public void Serialize(ResourceCount value, IObjectSaver objectSaver) {
      objectSaver.Set(InputOutputStockKey, value.InputOutputStock);
      objectSaver.Set(OutputStockKey, value.OutputStock);
      objectSaver.Set(InputOutputCapacityKey, value.InputOutputCapacity);
      objectSaver.Set(FillRateKey, value.FillRate);
    }

    public Obsoletable<ResourceCount> Deserialize(IObjectLoader objectLoader) {
      return (ResourceCount) _constructor.Invoke(new object[] {
          objectLoader.Get(InputOutputStockKey),
          objectLoader.Get(OutputStockKey),
          objectLoader.Get(InputOutputCapacityKey),
          objectLoader.Get(FillRateKey)
      });
    }

  }
}