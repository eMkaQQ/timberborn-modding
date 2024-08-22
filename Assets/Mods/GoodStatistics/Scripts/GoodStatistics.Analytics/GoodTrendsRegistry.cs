using GoodStatistics.Sampling;
using System.Collections.Generic;

namespace GoodStatistics.Analytics {
  public class GoodTrendsRegistry {

    private readonly IGoodTrendAnalyzer _goodTrendAnalyzer;
    private readonly GoodSamplesRegistry _goodSamplesRegistry;
    private readonly Dictionary<string, GoodTrend> _goodTrends = new();

    public GoodTrendsRegistry(IGoodTrendAnalyzer goodTrendAnalyzer,
                              GoodSamplesRegistry goodSamplesRegistry) {
      _goodTrendAnalyzer = goodTrendAnalyzer;
      _goodSamplesRegistry = goodSamplesRegistry;
    }

    public void Initialize() {
      _goodSamplesRegistry.SampleAdded += OnGoodSampleSampled;
      foreach (var goodSampleRecords in _goodSamplesRegistry.GoodSampleRecords) {
        _goodTrends[goodSampleRecords.GoodId] = new();
        Update(goodSampleRecords);
      }
    }

    public GoodTrend GetTrend(string goodId) {
      return _goodTrends[goodId];
    }

    private void OnGoodSampleSampled(object sender, GoodSampleRecords countRecords) {
      Update(countRecords);
    }

    private void Update(GoodSampleRecords countRecords) {
      _goodTrendAnalyzer.Analyze(countRecords, out var trendType, out var daysLeft);
      _goodTrends[countRecords.GoodId].Update(trendType, daysLeft);
    }

  }
}