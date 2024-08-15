using GoodStatistics.Core;
using GoodStatistics.Settings;
using UnityEngine;

namespace GoodStatistics.Analytics {
  internal class EMAAnalyzer : IGoodTrendAnalyzer {

    private readonly EMAAnalyzerSettings _emaAnalyzerSettings;

    public EMAAnalyzer(EMAAnalyzerSettings emaAnalyzerSettings) {
      _emaAnalyzerSettings = emaAnalyzerSettings;
    }

    public void Analyze(ResourceCountHistory resourceCountHistory, out TrendType trendType,
                        out float daysLeft) {
      if (WasAtFullOrZeroPreviously(resourceCountHistory)) {
        trendType = TrendType.Stable;
        daysLeft = -1;
        return;
      }
      GetTrendAndChange(resourceCountHistory, 0, out trendType, out var change);
      if (trendType == TrendType.Stable
          && !Mathf.Approximately(change, 0)
          && CheckConsecutiveMicroChanges(resourceCountHistory, change)) {
        trendType = change > 0 ? TrendType.LowGrowth : TrendType.LowDepletion;
      }
      if (trendType.IsDepleting()) {
        daysLeft = CountDaysLeftToZero(resourceCountHistory, change);
      } else if (trendType.IsGrowing()) {
        daysLeft = CountDaysLeftToFull(resourceCountHistory, change);
      } else {
        daysLeft = -1;
      }
    }

    private static bool WasAtFullOrZeroPreviously(ResourceCountHistory resourceCountHistory) {
      return resourceCountHistory.GoodSamples[1].FillRate is > 0.99f or < 0.01f;
    }

    private void GetTrendAndChange(ResourceCountHistory resourceCountHistory, int index,
                                   out TrendType trendType, out float change) {
      var indexChangeAverage = GetAverage(resourceCountHistory, index);
      var previousAverage = GetAverage(resourceCountHistory, index + 1);
      change = indexChangeAverage - previousAverage;
      var changePercentage = change / resourceCountHistory.GetMaxCapacity();
      trendType = ChangeToTrendType(changePercentage);
    }

    private float GetAverage(ResourceCountHistory resourceCountHistory, int index) {
      var samplesToAnalyze = Mathf.Clamp(_emaAnalyzerSettings.SamplesToAnalyze.Value,
                                         1, resourceCountHistory.GoodSamples.Count - index);
      var alpha = 2.0f / (_emaAnalyzerSettings.SamplesToAnalyze.Value + 1);
      float numerator = 0;
      float denominator = 0;
      for (var i = 0; i < samplesToAnalyze; i++) {
        var weight = Mathf.Pow(1 - alpha, i);
        numerator += resourceCountHistory.GoodSamples[i + index].TotalStock * weight;
        denominator += weight;
      }
      return numerator / denominator;
    }

    private TrendType ChangeToTrendType(float changePercentage) {
      if (changePercentage < 0) {
        if (changePercentage < -_emaAnalyzerSettings.HighChangeThreshold.Value / 100f) {
          return TrendType.HighDepletion;
        }
        if (changePercentage < -_emaAnalyzerSettings.MediumChangeThreshold.Value / 100f) {
          return TrendType.MediumDepletion;
        }
        if (changePercentage < -_emaAnalyzerSettings.LowChangeThreshold.Value / 100f) {
          return TrendType.LowDepletion;
        }
      } else {
        if (changePercentage > _emaAnalyzerSettings.HighChangeThreshold.Value / 100f) {
          return TrendType.HighGrowth;
        }
        if (changePercentage > _emaAnalyzerSettings.MediumChangeThreshold.Value / 100f) {
          return TrendType.MediumGrowth;
        }
        if (changePercentage > _emaAnalyzerSettings.LowChangeThreshold.Value / 100f) {
          return TrendType.LowGrowth;
        }
      }
      return TrendType.Stable;
    }

    private bool CheckConsecutiveMicroChanges(ResourceCountHistory resourceCountHistory,
                                              float changeToCheck) {
      for (var index = 0;
           index < _emaAnalyzerSettings.ConsecutiveMicroChangesThreshold.Value;
           index++) {
        GetTrendAndChange(resourceCountHistory, index + 1, out _, out var change);
        if (!Mathf.Approximately(Mathf.Sign(change), Mathf.Sign(changeToCheck))) {
          return false;
        }
      }
      return true;
    }

    private static float CountDaysLeftToZero(ResourceCountHistory resourceCountHistory,
                                             float change) {
      var changeTime = resourceCountHistory.GoodSamples[0].DayTimestamp
                       - resourceCountHistory.GoodSamples[1].DayTimestamp;
      var dailyChange = -change / changeTime;
      var stockLeft = resourceCountHistory.GoodSamples[0].TotalStock;
      return stockLeft / dailyChange;
    }

    private static float CountDaysLeftToFull(ResourceCountHistory resourceCountHistory,
                                             float change) {
      var changeTime = resourceCountHistory.GoodSamples[0].DayTimestamp
                       - resourceCountHistory.GoodSamples[1].DayTimestamp;
      var dailyChange = change / changeTime;
      var stockLeft = resourceCountHistory.GoodSamples[0].InputOutputCapacity
                      - resourceCountHistory.GoodSamples[0].TotalStock;
      return stockLeft / dailyChange;
    }

  }
}