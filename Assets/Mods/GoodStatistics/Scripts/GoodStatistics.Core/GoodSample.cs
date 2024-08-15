using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Core {
  public readonly struct GoodSample {

    public ResourceCount ResourceCount { get; }
    public float DayTimestamp { get; }

    public GoodSample(ResourceCount resourceCount,
                      float dayTimestamp) {
      ResourceCount = resourceCount;
      DayTimestamp = dayTimestamp;
    }

    public int InputOutputCapacity => ResourceCount.InputOutputCapacity;
    public int InputOutputStock => ResourceCount.InputOutputStock;
    public int TotalStock => ResourceCount.TotalStock;
    public float FillRate => ResourceCount.FillRate;

  }
}