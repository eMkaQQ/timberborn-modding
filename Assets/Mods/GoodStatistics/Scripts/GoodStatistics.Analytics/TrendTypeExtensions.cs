namespace GoodStatistics.Analytics {
  public static class TrendTypeExtensions {

    public static bool IsDepleting(this TrendType trendType) {
      return trendType is TrendType.LowDepletion or TrendType.MediumDepletion
                                                 or TrendType.HighDepletion;
    }

    public static bool IsGrowing(this TrendType trendType) {
      return trendType is TrendType.LowGrowth or TrendType.MediumGrowth
                                              or TrendType.HighGrowth;
    }

  }
}