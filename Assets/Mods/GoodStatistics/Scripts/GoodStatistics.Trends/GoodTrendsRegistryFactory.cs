using GoodStatistics.Analytics;
using GoodStatistics.Sampling;
using GoodStatistics.Settings;

namespace GoodStatistics.Trends {
  public class GoodTrendsRegistryFactory {

    private readonly ITrendAnalyzer _trendAnalyzer;
    private readonly GoodStatisticsSettings _goodStatisticsSettings;

    public GoodTrendsRegistryFactory(ITrendAnalyzer trendAnalyzer,
                                     GoodStatisticsSettings goodStatisticsSettings) {
      _trendAnalyzer = trendAnalyzer;
      _goodStatisticsSettings = goodStatisticsSettings;
    }

    public GoodTrendsRegistry Create(GoodSamplesRegistry goodSamplesRegistry) {
      var registry = new GoodTrendsRegistry(_trendAnalyzer, _goodStatisticsSettings,
                                            goodSamplesRegistry);
      registry.Initialize();
      return registry;
    }

  }
}