using Timberborn.Common;
using Timberborn.Goods;
using Timberborn.Persistence;

namespace Riverborne.Core {
  public class RaftDispatchSerializer : IValueSerializer<RaftDispatch> {

    private static readonly PropertyKey<string> NameKey = new("Name");
    private static readonly ListKey<GoodAmount> CargoKey = new("Cargo");
    private static readonly PropertyKey<float> IntervalKey = new("Interval");
    private static readonly PropertyKey<float> LastDispatchTimeKey = new("LastDispatchTime");
    private static readonly PropertyKey<bool> IsPausedKey = new("IsPaused");
    private readonly GoodAmountSerializer _goodAmountSerializer;

    public RaftDispatchSerializer(GoodAmountSerializer goodAmountSerializer) {
      _goodAmountSerializer = goodAmountSerializer;
    }

    public void Serialize(RaftDispatch value, IValueSaver valueSaver) {
      var objectSaver = valueSaver.AsObject();
      objectSaver.Set(NameKey, value.Name);
      objectSaver.Set(CargoKey, value.Cargo, _goodAmountSerializer);
      objectSaver.Set(IntervalKey, value.Interval);
      objectSaver.Set(LastDispatchTimeKey, value.LastDispatchTime);
      if (value.IsPaused) {
        objectSaver.Set(IsPausedKey, value.IsPaused);
      }
    }

    public Obsoletable<RaftDispatch> Deserialize(IValueLoader valueLoader) {
      var objectLoader = valueLoader.AsObject();
      var name = objectLoader.Get(NameKey);
      var cargo = objectLoader.Get(CargoKey, _goodAmountSerializer);
      var interval = objectLoader.Get(IntervalKey);
      var lastDispatchTime = objectLoader.Get(LastDispatchTimeKey);
      var isPaused = objectLoader.Has(IsPausedKey) && objectLoader.Get(IsPausedKey);
      return new(new(name, cargo, interval, lastDispatchTime, isPaused));
    }

  }
}