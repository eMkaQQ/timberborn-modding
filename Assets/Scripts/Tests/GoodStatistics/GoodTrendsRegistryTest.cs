using GoodStatistics.Analytics;
using GoodStatistics.GoodSampling;
using GoodStatistics.GoodTrends;
using GoodStatistics.Settings;
using NUnit.Framework;
using Timberborn.ResourceCountingSystem;

namespace Tests.GoodStatistics {
  public class GoodTrendsRegistryTest {

    private static readonly string BreadId = "Bread";

    [Test]
    public void ShouldAnalyzeSample() {
      // Given
      var goodSampleRegistry = GoodSamplesRegistry.CreateNew(new[] { BreadId });
      var settings = new GoodStatisticsSettings(null, null, null);
      var analyzer = new MockGoodTrendAnalyzer();
      var goodTrendsRegistryFactory = new GoodTrendsRegistryFactory(analyzer, settings);
      goodTrendsRegistryFactory.Create(goodSampleRegistry);
      settings.IgnoreGoodsWithStorageLessThan.SetValue(0);
      settings.AnalyzeGoodsWithoutStockpiles.SetValue(true);

      // When
      goodSampleRegistry.AddSample(BreadId, new(ResourceCount.Create(10, 10, 10, 10), 0));

      // Then
      Assert.IsTrue(analyzer.WasAnalyzeCalled);
    }

    [Test]
    public void ShouldNotAnalyzeSampleWithoutStockpile() {
      // Given
      var goodSampleRegistry = GoodSamplesRegistry.CreateNew(new[] { BreadId });
      var settings = new GoodStatisticsSettings(null, null, null);
      var analyzer = new MockGoodTrendAnalyzer();
      var goodTrendsRegistryFactory = new GoodTrendsRegistryFactory(analyzer, settings);
      goodTrendsRegistryFactory.Create(goodSampleRegistry);
      settings.IgnoreGoodsWithStorageLessThan.SetValue(0);
      settings.AnalyzeGoodsWithoutStockpiles.SetValue(false);

      // When
      goodSampleRegistry.AddSample(BreadId, new(ResourceCount.Create(0, 10, 0, 10), 0));

      // Then
      Assert.IsFalse(analyzer.WasAnalyzeCalled);
    }

    [Test]
    public void ShouldAnalyzeSampleWithoutStockpile() {
      // Given
      var goodSampleRegistry = GoodSamplesRegistry.CreateNew(new[] { BreadId });
      var settings = new GoodStatisticsSettings(null, null, null);
      var analyzer = new MockGoodTrendAnalyzer();
      var goodTrendsRegistryFactory = new GoodTrendsRegistryFactory(analyzer, settings);
      goodTrendsRegistryFactory.Create(goodSampleRegistry);
      settings.IgnoreGoodsWithStorageLessThan.SetValue(0);
      settings.AnalyzeGoodsWithoutStockpiles.SetValue(true);

      // When
      goodSampleRegistry.AddSample(BreadId, new(ResourceCount.Create(0, 10, 0, 10), 0));

      // Then
      Assert.IsTrue(analyzer.WasAnalyzeCalled);
    }

    [Test]
    public void ShouldNotAnalyzeSampleWithLowStockpile() {
      // Given
      var goodSampleRegistry = GoodSamplesRegistry.CreateNew(new[] { BreadId });
      var settings = new GoodStatisticsSettings(null, null, null);
      var analyzer = new MockGoodTrendAnalyzer();
      var goodTrendsRegistryFactory = new GoodTrendsRegistryFactory(analyzer, settings);
      goodTrendsRegistryFactory.Create(goodSampleRegistry);
      settings.IgnoreGoodsWithStorageLessThan.SetValue(10);
      settings.AnalyzeGoodsWithoutStockpiles.SetValue(false);

      // When
      goodSampleRegistry.AddSample(BreadId, new(ResourceCount.Create(5, 0, 5, 0), 0));

      // Then
      Assert.IsFalse(analyzer.WasAnalyzeCalled);
    }

    [Test]
    public void ShouldAnalyzeSampleWithEnoughStockpile() {
      // Given
      var goodSampleRegistry = GoodSamplesRegistry.CreateNew(new[] { BreadId });
      var settings = new GoodStatisticsSettings(null, null, null);
      var analyzer = new MockGoodTrendAnalyzer();
      var goodTrendsRegistryFactory = new GoodTrendsRegistryFactory(analyzer, settings);
      goodTrendsRegistryFactory.Create(goodSampleRegistry);
      settings.IgnoreGoodsWithStorageLessThan.SetValue(10);
      settings.AnalyzeGoodsWithoutStockpiles.SetValue(false);

      // When
      goodSampleRegistry.AddSample(BreadId, new(ResourceCount.Create(0, 0, 10, 0), 0));

      // Then
      Assert.IsTrue(analyzer.WasAnalyzeCalled);
    }

    private class MockGoodTrendAnalyzer : ITrendAnalyzer {

      public bool WasAnalyzeCalled { get; private set; }

      public void Analyze(ISampleRecords sampleRecords, out TrendType trendType,
                          out float daysLeft) {
        trendType = TrendType.HighGrowth;
        daysLeft = 500;
        WasAnalyzeCalled = true;
      }

    }

  }
}