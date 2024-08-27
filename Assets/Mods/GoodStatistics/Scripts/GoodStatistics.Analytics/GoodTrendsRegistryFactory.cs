using GoodStatistics.Sampling;
using GoodStatistics.Settings;

namespace GoodStatistics.Analytics {
  public class GoodTrendsRegistryFactory {

    private readonly IGoodTrendAnalyzer _goodTrendAnalyzer;
    private readonly GoodStatisticsSettings _goodStatisticsSettings;

    public GoodTrendsRegistryFactory(IGoodTrendAnalyzer goodTrendAnalyzer,
                                     GoodStatisticsSettings goodStatisticsSettings) {
      _goodTrendAnalyzer = goodTrendAnalyzer;
      _goodStatisticsSettings = goodStatisticsSettings;
    }

    public GoodTrendsRegistry Create(GoodSamplesRegistry goodSamplesRegistry) {
      var registry = new GoodTrendsRegistry(_goodTrendAnalyzer, _goodStatisticsSettings,
                                            goodSamplesRegistry);
      registry.Initialize();
      return registry;
    }

  }
}