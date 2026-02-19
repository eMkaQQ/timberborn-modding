using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Sampling {
  public readonly struct GoodSample {

    public ResourceCount ResourceCount { get; }
    public float DayTimestamp { get; }
    public int TotalCapacity { get; }

    public GoodSample(ResourceCount resourceCount,
                      float dayTimestamp) {
      ResourceCount = resourceCount;
      DayTimestamp = dayTimestamp;
      TotalCapacity = CalculateTotalCapacity(ResourceCount);
    }

    public int InputOutputCapacity => ResourceCount.InputOutputCapacity;
    public int TotalStock => ResourceCount.TotalStock;
    public float FillRate => ResourceCount.FillRate;

    private static int CalculateTotalCapacity(ResourceCount resourceCount) {
      if (resourceCount.FillRate == 0) {
        return resourceCount.InputOutputCapacity;
      }
      return (int) (resourceCount.TotalStock / resourceCount.FillRate);
    }

  }
}