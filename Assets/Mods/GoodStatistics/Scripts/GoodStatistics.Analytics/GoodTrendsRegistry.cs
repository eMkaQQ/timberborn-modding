﻿using GoodStatistics.Sampling;
using GoodStatistics.Settings;
using System.Collections.Generic;

namespace GoodStatistics.Analytics {
  public class GoodTrendsRegistry {

    private readonly IGoodTrendAnalyzer _goodTrendAnalyzer;
    private readonly GoodStatisticsSettings _goodStatisticsSettings;
    private readonly GoodSamplesRegistry _goodSamplesRegistry;
    private readonly Dictionary<string, GoodTrend> _goodTrends = new();

    public GoodTrendsRegistry(IGoodTrendAnalyzer goodTrendAnalyzer,
                              GoodStatisticsSettings goodStatisticsSettings,
                              GoodSamplesRegistry goodSamplesRegistry) {
      _goodTrendAnalyzer = goodTrendAnalyzer;
      _goodStatisticsSettings = goodStatisticsSettings;
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

    private void Update(GoodSampleRecords goodSampleRecords) {
      if (ShouldBeAnalyzed(goodSampleRecords)) {
        _goodTrendAnalyzer.Analyze(goodSampleRecords, out var trendType, out var daysLeft);
        _goodTrends[goodSampleRecords.GoodId].Update(trendType, daysLeft);
      } else {
        _goodTrends[goodSampleRecords.GoodId].Update(TrendType.Stable, -1);
      }
    }

    private bool ShouldBeAnalyzed(GoodSampleRecords goodSampleRecords) {
      var lastSample = goodSampleRecords.GoodSamples[0];
      return (_goodStatisticsSettings.AnalyzeGoodsWithoutStockpiles.Value
              || lastSample.InputOutputCapacity > 0)
             && lastSample.InputOutputCapacity
             >= _goodStatisticsSettings.IgnoreGoodsWithStorageLessThan.Value;
    }

  }
}