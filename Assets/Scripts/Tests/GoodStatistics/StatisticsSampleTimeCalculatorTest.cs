using GoodStatistics.Core;
using GoodStatistics.Settings;
using NUnit.Framework;
using System;
using Timberborn.TimeSystem;

namespace Tests.GoodStatistics {
  public class StatisticsSampleTimeCalculatorTest {

    [Test]
    public void ShouldCorrectlyCalculateNextSampleTime() {
      // Arrange
      var goodStatisticsSettings = new GoodStatisticsSettings(null, null, null);
      var samplesPerDay = goodStatisticsSettings.SamplesPerDay;
      var dayNightCycle = new DayNightCycleMock();
      var statisticsSampleTimeCalculator =
          new StatisticsSampleTimeCalculator(goodStatisticsSettings, dayNightCycle);

      samplesPerDay.SetValue(1);
      dayNightCycle.PartialDayNumber = 0;
      Assert.AreEqual(0, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.5f;
      Assert.AreEqual(1, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1;
      Assert.AreEqual(1, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.2f;
      Assert.AreEqual(26, statisticsSampleTimeCalculator.CalculateNextSampleTime());

      samplesPerDay.SetValue(2);
      dayNightCycle.PartialDayNumber = 0;
      Assert.AreEqual(0, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.25f;
      Assert.AreEqual(0.5f, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.5f;
      Assert.AreEqual(0.5f, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.75f;
      Assert.AreEqual(1, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1;
      Assert.AreEqual(1, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.2f;
      Assert.AreEqual(25.5f, statisticsSampleTimeCalculator.CalculateNextSampleTime());

      samplesPerDay.SetValue(24);
      dayNightCycle.PartialDayNumber = 0;
      Assert.AreEqual(0, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.2f;
      Assert.AreEqual(5 * (1 / 24f), statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.6f;
      Assert.AreEqual(15 * (1 / 24f), statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1;
      Assert.AreEqual(1, statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.7f;
      Assert.AreEqual(25 + 17 * (1 / 24f),
                      statisticsSampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.99f;
      Assert.AreEqual(26, statisticsSampleTimeCalculator.CalculateNextSampleTime());
    }

    private class DayNightCycleMock : IDayNightCycle {

      public int ConfiguredDayLengthInSeconds { get; }
      public int DayNumber { get; }
      public float DaytimeLengthInHours { get; }
      public float NighttimeLengthInHours { get; }
      public float HoursPassedToday { get; }
      public float DayProgress { get; }
      public float PartialDayNumber { get; set; }
      public TimeOfDay FluidTimeOfDay { get; }
      public bool IsDaytime { get; }
      public bool IsNighttime { get; }
      public float FixedDeltaTimeInHours { get; }

      public float DayNumberHoursFromNow(float hours) {
        throw new NotSupportedException();
      }

      public (float start, float end) BoundsInHours(TimeOfDay timeOfDay) {
        throw new NotSupportedException();
      }

      public float HoursToNextStartOf(TimeOfDay timeOfDay) {
        throw new NotSupportedException();
      }

      public float SecondsToHours(float seconds) {
        throw new NotSupportedException();
      }

      public float FluidHoursToNextStartOf(TimeOfDay timeOfDay) {
        throw new NotSupportedException();
      }

      public void SetTimeToNextDay() {
        throw new NotSupportedException();
      }

      public void JumpTimeInHours(float hours) {
        throw new NotSupportedException();
      }

    }

  }
}