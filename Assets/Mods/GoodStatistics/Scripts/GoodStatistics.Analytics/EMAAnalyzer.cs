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
      GetTrendAndDailyChange(goodSampleRecords, 0, out trendType, out var dailyChange);
      if (trendType == TrendType.Stable
          && CheckConsecutiveMicroChanges(goodSampleRecords, dailyChange)) {
        trendType = dailyChange > 0 ? TrendType.LowGrowth : TrendType.LowDepletion;
      }
      if (trendType.IsDepleting()) {
        daysLeft = CountDaysLeftToZero(goodSampleRecords, dailyChange);
      } else if (trendType.IsGrowing()) {
        daysLeft = CountDaysLeftToFull(goodSampleRecords, dailyChange);
      } else {
        daysLeft = -1;
      }
    }

    private static bool WasAtFullOrZeroPreviously(GoodSampleRecords goodSampleRecords) {
      return goodSampleRecords.GoodSamples[1].FillRate is > 0.999f or < 0.001f;
    }

    private void GetTrendAndDailyChange(GoodSampleRecords goodSampleRecords, int index,
                                        out TrendType trendType, out float dailyChange) {
      var indexChangeAverage = GetAverage(goodSampleRecords, index);
      var previousAverage = GetAverage(goodSampleRecords, index + 1);
      var change = indexChangeAverage - previousAverage;
      var changeTime = goodSampleRecords.GoodSamples[0].DayTimestamp
                       - goodSampleRecords.GoodSamples[1].DayTimestamp;
      dailyChange = changeTime == 0 || goodSampleRecords.GoodSamples[1].DayTimestamp < 0
          ? 0
          : change / changeTime;
      var changePercentage = dailyChange / goodSampleRecords.GetMaxCapacity();
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
      var samplesToCheck = _emaAnalyzerSettings.ConsecutiveMicroChangesThreshold.Value;
      var maxIndex = goodSampleRecords.GoodSamples.Count - 1;
      var sameSignCounter = 0;
      for (var index = 0; index < samplesToCheck && index < maxIndex; index++) {
        GetTrendAndDailyChange(goodSampleRecords, index + 1, out _, out var dailyChange);
        if (Mathf.Approximately(dailyChange, 0)) {
          samplesToCheck++;
        } else if (!Mathf.Approximately(Mathf.Sign(dailyChange), Mathf.Sign(changeToCheck))) {
          return false;
        } else {
          sameSignCounter++;
        }
      }
      return sameSignCounter >= _emaAnalyzerSettings.ConsecutiveMicroChangesThreshold.Value;
    }

    private static float CountDaysLeftToZero(GoodSampleRecords goodSampleRecords,
                                             float dailyChange) {
      var stockLeft = goodSampleRecords.GoodSamples[0].TotalStock;
      return stockLeft / -dailyChange;
    }

    private static float CountDaysLeftToFull(GoodSampleRecords goodSampleRecords,
                                             float dailyChange) {
      var capacityLeft = goodSampleRecords.GoodSamples[0].TotalCapacity
                         - goodSampleRecords.GoodSamples[0].TotalStock;
      return capacityLeft / dailyChange;
    }

  }
}