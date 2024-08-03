using Timberborn.SingletonSystem;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;

namespace GoodStatistics.Core {
  internal class GoodStatisticsSampleTrigger : ITickableSingleton,
                                               ILoadableSingleton {

    private readonly StatisticsSampleTimeCalculator _statisticsSampleTimeCalculator;
    private readonly IDayNightCycle _dayNightCycle;
    private readonly GoodStatisticsSampler _goodStatisticsSampler;
    private float _nextSampleTime;

    public GoodStatisticsSampleTrigger(StatisticsSampleTimeCalculator 
                                           statisticsSampleTimeCalculator,
                                       IDayNightCycle dayNightCycle,
                                       GoodStatisticsSampler goodStatisticsSampler) {
      _statisticsSampleTimeCalculator = statisticsSampleTimeCalculator;
      _dayNightCycle = dayNightCycle;
      _goodStatisticsSampler = goodStatisticsSampler;
    }

    public void Tick() {
      if (_dayNightCycle.PartialDayNumber >= _nextSampleTime) {
        _goodStatisticsSampler.CollectGoodsSamples();
        CalculateNextSampleTime();
      }
    }

    public void Load() {
      CalculateNextSampleTime();
    }

    private void CalculateNextSampleTime() {
      _nextSampleTime = _statisticsSampleTimeCalculator.CalculateNextSampleTime();
    }

  }
}