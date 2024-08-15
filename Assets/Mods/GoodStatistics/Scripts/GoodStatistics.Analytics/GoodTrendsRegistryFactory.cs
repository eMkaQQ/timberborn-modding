using GoodStatistics.Core;

namespace GoodStatistics.Analytics {
  internal class GoodTrendsRegistryFactory {

    private readonly IGoodTrendAnalyzer _goodTrendAnalyzer;

    public GoodTrendsRegistryFactory(IGoodTrendAnalyzer goodTrendAnalyzer) {
      _goodTrendAnalyzer = goodTrendAnalyzer;
    }

    public GoodTrendsRegistry Create(ResourceCountsRegistry resourceCountsRegistry) {
      var registry = new GoodTrendsRegistry(_goodTrendAnalyzer, resourceCountsRegistry);
      registry.Initialize();
      return registry;
    }

  }
}