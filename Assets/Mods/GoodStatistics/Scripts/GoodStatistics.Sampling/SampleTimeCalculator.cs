using GoodStatistics.Settings;
using Timberborn.TimeSystem;
using UnityEngine;

namespace GoodStatistics.Sampling {
  internal class SampleTimeCalculator {

    private readonly GoodStatisticsSettings _goodStatisticsSettings;
    private readonly IDayNightCycle _dayNightCycle;

    public SampleTimeCalculator(GoodStatisticsSettings goodStatisticsSettings,
                                IDayNightCycle dayNightCycle) {
      _goodStatisticsSettings = goodStatisticsSettings;
      _dayNightCycle = dayNightCycle;
    }

    public float CalculateNextSampleTime() {
      var partialDayNumber = _dayNightCycle.PartialDayNumber;
      var samplesPerDay = _goodStatisticsSettings.SamplesPerDay.Value;
      var sampleInterval = 1f / samplesPerDay;
      return Mathf.Ceil(partialDayNumber / sampleInterval) * sampleInterval;
    }

  }
}