using GoodStatistics.Settings;
using UnityEngine;

namespace GoodStatistics.Analytics {
  internal class EMAAnalyzer : ITrendAnalyzer {

    private readonly EMAAnalyzerSettings _emaAnalyzerSettings;

    public EMAAnalyzer(EMAAnalyzerSettings emaAnalyzerSettings) {
      _emaAnalyzerSettings = emaAnalyzerSettings;
    }

    public void Analyze(ISampleRecords sampleRecords, out TrendType trendType) {
      if (sampleRecords.WasAtFullOrZeroPreviously()) {
        trendType = TrendType.Stable;
        return;
      }
      GetTrendAndDailyChange(sampleRecords, 0, out trendType, out var dailyChange);
      if (trendType == TrendType.Stable
          && CheckConsecutiveMicroChanges(sampleRecords, dailyChange)) {
        trendType = dailyChange > 0 ? TrendType.LowGrowth : TrendType.LowDepletion;
      }
    }

    public void Analyze(ISampleRecords sampleRecords, out TrendType trendType,
                        out float daysLeft) {
      if (sampleRecords.WasAtFullOrZeroPreviously()) {
        trendType = TrendType.Stable;
        daysLeft = -1;
        return;
      }
      GetTrendAndDailyChange(sampleRecords, 0, out trendType, out var dailyChange);
      if (trendType == TrendType.Stable
          && CheckConsecutiveMicroChanges(sampleRecords, dailyChange)) {
        trendType = dailyChange > 0 ? TrendType.LowGrowth : TrendType.LowDepletion;
      }
      if (trendType.IsDepleting()) {
        daysLeft = CountDaysLeftToZero(sampleRecords, dailyChange);
      } else if (trendType.IsGrowing()) {
        daysLeft = CountDaysLeftToFull(sampleRecords, dailyChange);
      } else {
        daysLeft = -1;
      }
    }

    private void GetTrendAndDailyChange(ISampleRecords sampleRecords, int index,
                                        out TrendType trendType, out float dailyChange) {
      var indexChangeAverage = GetAverage(sampleRecords, index);
      var previousAverage = GetAverage(sampleRecords, index + 1);
      var change = indexChangeAverage - previousAverage;
      var changeTime = sampleRecords.GetDayTimestampAt(0)
                       - sampleRecords.GetDayTimestampAt(1);
      dailyChange = changeTime == 0 || sampleRecords.GetDayTimestampAt(1) < 0
          ? 0
          : change / changeTime;
      var changePercentage = dailyChange / sampleRecords.GetMaxCapacity();
      trendType = ChangeToTrendType(changePercentage);
    }

    private float GetAverage(ISampleRecords sampleRecords, int index) {
      var samplesToAnalyze = Mathf.Clamp(_emaAnalyzerSettings.SamplesToAnalyze.Value,
                                         1, sampleRecords.GetSamplesCount() - index);
      var alpha = 2.0f / (_emaAnalyzerSettings.SamplesToAnalyze.Value + 1);
      float numerator = 0;
      float denominator = 0;
      for (var i = 0; i < samplesToAnalyze; i++) {
        var weight = Mathf.Pow(1 - alpha, i);
        numerator += sampleRecords.GetTotalAmountAt(i + index) * weight;
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

    private bool CheckConsecutiveMicroChanges(ISampleRecords sampleRecords, float changeToCheck) {
      var samplesToCheck = _emaAnalyzerSettings.ConsecutiveMicroChangesThreshold.Value;
      var maxIndex = sampleRecords.GetSamplesCount() - 1;
      var sameSignCounter = 0;
      for (var index = 0; index < samplesToCheck && index < maxIndex; index++) {
        GetTrendAndDailyChange(sampleRecords, index + 1, out _, out var dailyChange);
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

    private static float CountDaysLeftToZero(ISampleRecords sampleRecords, float dailyChange) {
      var stockLeft = sampleRecords.GetTotalAmountAt(0);
      return stockLeft / -dailyChange;
    }

    private static float CountDaysLeftToFull(ISampleRecords sampleRecords, float dailyChange) {
      var capacityLeft = sampleRecords.GetTotalCapacityAt(0)
                         - sampleRecords.GetTotalAmountAt(0);
      return capacityLeft / dailyChange;
    }

  }
}