using GoodStatistics.Sampling;
using GoodStatistics.Settings;
using UnityEngine;

namespace GoodStatistics.Analytics {
  internal class EMAAnalyzer : IGoodTrendAnalyzer {

    private readonly EMAAnalyzerSettings _emaAnalyzerSettings;

    public EMAAnalyzer(EMAAnalyzerSettings emaAnalyzerSettings) {
      _emaAnalyzerSettings = emaAnalyzerSettings;
    }

    public void Analyze(GoodSampleRecords goodSampleRecords, out TrendType trendType,
                        out float daysLeft) {
      if (WasAtFullOrZeroPreviously(goodSampleRecords)) {
        trendType = TrendType.Stable;
        daysLeft = -1;
        return;
      }
      GetTrendAndChange(goodSampleRecords, 0, out trendType, out var change);
      if (trendType == TrendType.Stable
          && !Mathf.Approximately(change, 0)
          && CheckConsecutiveMicroChanges(goodSampleRecords, change)) {
        trendType = change > 0 ? TrendType.LowGrowth : TrendType.LowDepletion;
      }
      if (trendType.IsDepleting()) {
        daysLeft = CountDaysLeftToZero(goodSampleRecords, change);
      } else if (trendType.IsGrowing()) {
        daysLeft = CountDaysLeftToFull(goodSampleRecords, change);
      } else {
        daysLeft = -1;
      }
    }

    private static bool WasAtFullOrZeroPreviously(GoodSampleRecords goodSampleRecords) {
      return goodSampleRecords.GoodSamples[1].FillRate is > 0.999f or < 0.001f;
    }

    private void GetTrendAndChange(GoodSampleRecords goodSampleRecords, int index,
                                   out TrendType trendType, out float change) {
      var indexChangeAverage = GetAverage(goodSampleRecords, index);
      var previousAverage = GetAverage(goodSampleRecords, index + 1);
      change = indexChangeAverage - previousAverage;
      var changePercentage = change / goodSampleRecords.GetMaxCapacity();
      trendType = ChangeToTrendType(changePercentage);
    }

    private float GetAverage(GoodSampleRecords goodSampleRecords, int index) {
      var samplesToAnalyze = Mathf.Clamp(_emaAnalyzerSettings.SamplesToAnalyze.Value,
                                         1, goodSampleRecords.GoodSamples.Count - index);
      var alpha = 2.0f / (_emaAnalyzerSettings.SamplesToAnalyze.Value + 1);
      float numerator = 0;
      float denominator = 0;
      for (var i = 0; i < samplesToAnalyze; i++) {
        var weight = Mathf.Pow(1 - alpha, i);
        numerator += goodSampleRecords.GoodSamples[i + index].TotalStock * weight;
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

    private bool CheckConsecutiveMicroChanges(GoodSampleRecords goodSampleRecords,
                                              float changeToCheck) {
      for (var index = 0;
           index < _emaAnalyzerSettings.ConsecutiveMicroChangesThreshold.Value;
           index++) {
        GetTrendAndChange(goodSampleRecords, index + 1, out _, out var change);
        if (!Mathf.Approximately(Mathf.Sign(change), Mathf.Sign(changeToCheck))) {
          return false;
        }
      }
      return true;
    }

    private static float CountDaysLeftToZero(GoodSampleRecords goodSampleRecords,
                                             float change) {
      var changeTime = goodSampleRecords.GoodSamples[0].DayTimestamp
                       - goodSampleRecords.GoodSamples[1].DayTimestamp;
      var dailyChange = -change / changeTime;
      var stockLeft = goodSampleRecords.GoodSamples[0].TotalStock;
      return stockLeft / dailyChange;
    }

    private static float CountDaysLeftToFull(GoodSampleRecords goodSampleRecords,
                                             float change) {
      var changeTime = goodSampleRecords.GoodSamples[0].DayTimestamp
                       - goodSampleRecords.GoodSamples[1].DayTimestamp;
      var dailyChange = change / changeTime;
      var stockLeft = goodSampleRecords.GoodSamples[0].InputOutputCapacity
                      - goodSampleRecords.GoodSamples[0].TotalStock;
      return stockLeft / dailyChange;
    }

  }
}