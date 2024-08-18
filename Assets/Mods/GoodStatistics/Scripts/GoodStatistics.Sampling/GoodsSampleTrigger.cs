using Timberborn.SingletonSystem;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;

namespace GoodStatistics.Sampling {
  internal class GoodsSampleTrigger : ITickableSingleton,
                                      ILoadableSingleton {

    private readonly SampleTimeCalculator _sampleTimeCalculator;
    private readonly IDayNightCycle _dayNightCycle;
    private readonly GoodsSampler _goodsSampler;
    private float _nextSampleTime;

    public GoodsSampleTrigger(SampleTimeCalculator sampleTimeCalculator,
                              IDayNightCycle dayNightCycle,
                              GoodsSampler goodsSampler) {
      _sampleTimeCalculator = sampleTimeCalculator;
      _dayNightCycle = dayNightCycle;
      _goodsSampler = goodsSampler;
    }

    public void Tick() {
      if (_dayNightCycle.PartialDayNumber >= _nextSampleTime) {
        _goodsSampler.CollectGoodsSamples();
        CalculateNextSampleTime();
      }
    }

    public void Load() {
      CalculateNextSampleTime();
    }

    private void CalculateNextSampleTime() {
      _nextSampleTime = _sampleTimeCalculator.CalculateNextSampleTime();
    }

  }
}