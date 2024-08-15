using GoodStatistics.Core;
using System.Collections.Generic;

namespace GoodStatistics.Analytics {
  internal class GoodTrendsRegistry {

    private readonly IGoodTrendAnalyzer _goodTrendAnalyzer;
    private readonly ResourceCountsRegistry _resourceCountsRegistry;
    private readonly Dictionary<string, GoodTrend> _goodTrends = new();

    public GoodTrendsRegistry(IGoodTrendAnalyzer goodTrendAnalyzer,
                              ResourceCountsRegistry resourceCountsRegistry) {
      _goodTrendAnalyzer = goodTrendAnalyzer;
      _resourceCountsRegistry = resourceCountsRegistry;
    }

    public void Initialize() {
      _resourceCountsRegistry.SampleAdded += OnResourceCountSampled;
      foreach (var resourceCountHistory in _resourceCountsRegistry.ResourceCountHistories) {
        _goodTrends[resourceCountHistory.GoodId] = new();
        Update(resourceCountHistory);
      }
    }

    private void OnResourceCountSampled(object sender, ResourceCountHistory countHistory) {
      Update(countHistory);
    }

    private void Update(ResourceCountHistory countHistory) {
      _goodTrendAnalyzer.Analyze(countHistory, out var trendType, out var daysLeft);
      _goodTrends[countHistory.GoodId].Update(trendType, daysLeft);
    }

  }
}