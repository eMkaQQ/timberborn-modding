using GoodStatistics.Core;

namespace GoodStatistics.Analytics {
  public interface IGoodTrendAnalyzer {

    void Analyze(ResourceCountHistory resourceCountHistory, out TrendType trendType,
                 out float daysLeft);

  }
}