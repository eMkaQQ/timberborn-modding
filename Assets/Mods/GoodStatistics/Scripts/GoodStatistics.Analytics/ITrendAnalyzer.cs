namespace GoodStatistics.Analytics {
  public interface ITrendAnalyzer {

    void Analyze(ISampleRecords sampleRecords, out TrendType trendType, out float daysLeft);

  }
}