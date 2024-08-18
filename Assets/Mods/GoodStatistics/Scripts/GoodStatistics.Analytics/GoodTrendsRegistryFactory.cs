using GoodStatistics.Sampling;

namespace GoodStatistics.Analytics {
  internal class GoodTrendsRegistryFactory {

    private readonly IGoodTrendAnalyzer _goodTrendAnalyzer;

    public GoodTrendsRegistryFactory(IGoodTrendAnalyzer goodTrendAnalyzer) {
      _goodTrendAnalyzer = goodTrendAnalyzer;
    }

    public GoodTrendsRegistry Create(GoodSamplesRegistry goodSamplesRegistry) {
      var registry = new GoodTrendsRegistry(_goodTrendAnalyzer, goodSamplesRegistry);
      registry.Initialize();
      return registry;
    }

  }
}