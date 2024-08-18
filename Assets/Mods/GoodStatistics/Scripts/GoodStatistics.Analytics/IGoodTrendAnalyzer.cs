using GoodStatistics.Sampling;

namespace GoodStatistics.Analytics {
  public interface IGoodTrendAnalyzer {

    void Analyze(GoodSampleRecords goodSampleRecords, out TrendType trendType,
                 out float daysLeft);

  }
}