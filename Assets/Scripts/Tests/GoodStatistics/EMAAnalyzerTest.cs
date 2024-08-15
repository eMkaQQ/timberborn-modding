using GoodStatistics.Analytics;
using GoodStatistics.Core;
using GoodStatistics.Settings;
using NUnit.Framework;
using Timberborn.ResourceCountingSystem;

namespace Tests.GoodStatistics {
  public class EMAAnalyzerTest {

    [Test]
    public void ShouldAnalyzeHighGrowingTrend() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(20, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(40, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(60, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.HighGrowth, trendType);
    }

    [Test]
    public void ShouldAnalyzeLowDepletingTrend() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(75, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(70, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(65, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.LowDepletion, trendType);
    }

    [Test]
    public void ShouldAnalyzeConsecutiveMicroChanges() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(79, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(78, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(77, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(76, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(75, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(74, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.LowDepletion, trendType);
    }

    [Test]
    public void ShouldAnalyzeNonChangingTrend() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.Stable, trendType);
    }

    [Test]
    public void ShouldAnalyzeChangingStableTrend() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(85, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(90, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(85, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(85, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(90, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(85, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.Stable, trendType);
    }

    [Test]
    public void ShouldAnalyzeTrendChange() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(20, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(40, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(60, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      Assert.AreEqual(TrendType.HighGrowth, trendType);
      resourceCountHistory.Add(new(ResourceCount.Create(70, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(55, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(40, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out trendType, out _);
      // Then
      Assert.AreEqual(TrendType.MediumDepletion, trendType);
    }

    [Test]
    public void ShouldIgnoreFirstChangeFromZeroState() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(0, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(0, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(0, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(0, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(20, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.Stable, trendType);
    }

    [Test]
    public void ShouldIgnoreFirstChangeFromFullState() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(100, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(100, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(100, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(100, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 0));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out _);
      // Then
      Assert.AreEqual(TrendType.Stable, trendType);
    }

    [Test]
    public void ShouldAnalyzeDaysLeftWhenDepleting() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 1));
      resourceCountHistory.Add(new(ResourceCount.Create(75, 0, 100, 0), 2));
      resourceCountHistory.Add(new(ResourceCount.Create(70, 0, 100, 0), 3));
      resourceCountHistory.Add(new(ResourceCount.Create(65, 0, 100, 0), 4));
      resourceCountHistory.Add(new(ResourceCount.Create(60, 0, 100, 0), 5));
      resourceCountHistory.Add(new(ResourceCount.Create(55, 0, 100, 0), 6));
      resourceCountHistory.Add(new(ResourceCount.Create(50, 0, 100, 0), 7));
      resourceCountHistory.Add(new(ResourceCount.Create(45, 0, 100, 0), 8));
      resourceCountHistory.Add(new(ResourceCount.Create(40, 0, 100, 0), 9));
      resourceCountHistory.Add(new(ResourceCount.Create(35, 0, 100, 0), 10));
      resourceCountHistory.Add(new(ResourceCount.Create(30, 0, 100, 0), 11));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out var daysLeft);
      // Then
      Assert.AreEqual(TrendType.MediumDepletion, trendType);
      Assert.AreEqual(6, daysLeft);
    }

    [Test]
    public void ShouldAnalyzeDaysLeftWhenMicroDepleting() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(70, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(69, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(68, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(67, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(66, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(65, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(64, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(63, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(62, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(61, 0, 100, 0), 4.25f));
      resourceCountHistory.Add(new(ResourceCount.Create(60, 0, 100, 0), 4.5f));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out var daysLeft);
      // Then
      Assert.AreEqual(TrendType.LowDepletion, trendType);
      Assert.IsTrue(15 - daysLeft < 0.01f);
    }

    [Test]
    public void ShouldAnalyzeDaysLeftWhenGrowing() {
      // Given
      var emaAnalyzerSettings = CreateEMAAnalyzerSettings();
      var emaAnalyzer = new EMAAnalyzer(emaAnalyzerSettings);
      var resourceCountHistory = ResourceCountHistory.CreateNew("Bread");
      // When
      resourceCountHistory.Add(new(ResourceCount.Create(30, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(35, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(40, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(45, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(50, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(55, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(60, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(65, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(70, 0, 100, 0), 0));
      resourceCountHistory.Add(new(ResourceCount.Create(75, 0, 100, 0), 1));
      resourceCountHistory.Add(new(ResourceCount.Create(80, 0, 100, 0), 2));
      emaAnalyzer.Analyze(resourceCountHistory, out var trendType, out var daysLeft);
      // Then
      Assert.AreEqual(TrendType.MediumGrowth, trendType);
      Assert.IsTrue(4 - daysLeft < 0.001f);
    }

    private static EMAAnalyzerSettings CreateEMAAnalyzerSettings() {
      var settings = new EMAAnalyzerSettings(null, null, null);
      settings.HighChangeThreshold.SetValue(10);
      settings.MediumChangeThreshold.SetValue(5);
      settings.LowChangeThreshold.SetValue(1);
      settings.SamplesToAnalyze.SetValue(5);
      settings.ConsecutiveMicroChangesThreshold.SetValue(5);
      return settings;
    }

  }
}