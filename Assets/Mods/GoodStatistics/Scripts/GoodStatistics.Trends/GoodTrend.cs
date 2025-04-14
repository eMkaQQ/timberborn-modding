using GoodStatistics.Analytics;

namespace GoodStatistics.Trends {
  public class GoodTrend {

    public TrendType TrendType { get; private set; } = TrendType.Stable;
    public float DaysLeft { get; private set; } = float.MaxValue;

    public void Update(TrendType trendType, float daysLeft) {
      TrendType = trendType;
      DaysLeft = daysLeft;
    }

  }
}