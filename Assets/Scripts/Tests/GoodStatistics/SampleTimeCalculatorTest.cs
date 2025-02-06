using GoodStatistics.Sampling;
using GoodStatistics.Settings;
using NUnit.Framework;
using System;
using Timberborn.TimeSystem;

namespace Tests.GoodStatistics {
  public class SampleTimeCalculatorTest {

    [Test]
    public void ShouldCorrectlyCalculateNextSampleTime() {
      var goodStatisticsSettings = new GoodStatisticsSettings(null, null, null);
      var samplesPerDay = goodStatisticsSettings.SamplesPerDay;
      var dayNightCycle = new DayNightCycleMock();
      var sampleTimeCalculator = new SampleTimeCalculator(goodStatisticsSettings, dayNightCycle);

      samplesPerDay.SetValue(1);
      dayNightCycle.PartialDayNumber = 0;
      Assert.AreEqual(1, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.5f;
      Assert.AreEqual(1, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1;
      Assert.AreEqual(2, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1.99f;
      Assert.AreEqual(2, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.2f;
      Assert.AreEqual(26, sampleTimeCalculator.CalculateNextSampleTime());

      samplesPerDay.SetValue(2);
      dayNightCycle.PartialDayNumber = 0;
      Assert.AreEqual(0.5f, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.25f;
      Assert.AreEqual(0.5f, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.499f;
      Assert.AreEqual(0.5f, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.5f;
      Assert.AreEqual(1, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.75f;
      Assert.AreEqual(1, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1;
      Assert.AreEqual(1.5f, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.2f;
      Assert.AreEqual(25.5f, sampleTimeCalculator.CalculateNextSampleTime());

      samplesPerDay.SetValue(12);
      dayNightCycle.PartialDayNumber = 0;
      Assert.AreEqual(1 / 12f, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.2f;
      Assert.AreEqual(3 * (1 / 12f), sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 0.6f;
      Assert.AreEqual(8 * (1 / 12f), sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 1;
      Assert.AreEqual(1 + 1 / 12f, sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.7f;
      Assert.AreEqual(25 + 9 * (1 / 12f),
                      sampleTimeCalculator.CalculateNextSampleTime());
      dayNightCycle.PartialDayNumber = 25.99f;
      Assert.AreEqual(26, sampleTimeCalculator.CalculateNextSampleTime());
    }

    [Test]
    public void ShouldIgnoreFloatingPointErrors() {
      var goodStatisticsSettings = new GoodStatisticsSettings(null, null, null);
      var samplesPerDay = goodStatisticsSettings.SamplesPerDay;
      var dayNightCycle = new DayNightCycleMock();
      var sampleTimeCalculator = new SampleTimeCalculator(goodStatisticsSettings, dayNightCycle);

      samplesPerDay.SetValue(6);
      dayNightCycle.PartialDayNumber = 0.1f;
      var nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
      dayNightCycle.PartialDayNumber = nextSampleTime;
      nextSampleTime = sampleTimeCalculator.CalculateNextSampleTime();
      Assert.Greater(nextSampleTime, dayNightCycle.PartialDayNumber);
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