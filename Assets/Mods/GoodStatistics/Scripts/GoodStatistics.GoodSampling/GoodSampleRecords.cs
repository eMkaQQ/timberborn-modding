using GoodStatistics.Analytics;
using GoodStatistics.Settings;
using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.GoodSampling {
  public class GoodSampleRecords : ISampleRecords {

    public string GoodId { get; }
    private readonly List<GoodSample> _goodSamples;

    private GoodSampleRecords(string goodId,
                              List<GoodSample> goodSamples) {
      GoodId = goodId;
      _goodSamples = goodSamples;
    }

    public static GoodSampleRecords CreateNew(string goodId) {
      var goodSamples = Enumerable.Repeat(new GoodSample(ResourceCount.Create(0, 0, 0, 0), -1),
                                          GoodStatisticsConstants.MaxSamples).ToList();
      return new(goodId, goodSamples);
    }

    public static GoodSampleRecords CreateFromSave(string goodId,
                                                   List<GoodSample> goodSamples) {
      return new(goodId, goodSamples);
    }

    public ReadOnlyList<GoodSample> GoodSamples => new(_goodSamples);

    public void Add(GoodSample goodSample) {
      _goodSamples.Insert(0, goodSample);
      if (_goodSamples.Count > GoodStatisticsConstants.MaxSamples) {
        _goodSamples.RemoveAt(_goodSamples.Count - 1);
      }
      ReplaceMissingSamples(goodSample);
    }

    public int GetSamplesCount() {
      return _goodSamples.Count;
    }

    public float GetDayTimestampAt(int index) {
      return _goodSamples[index].DayTimestamp;
    }

    public int GetTotalAmountAt(int index) {
      return _goodSamples[index].TotalStock;
    }

    public int GetTotalCapacityAt(int index) {
      return _goodSamples[index].TotalCapacity;
    }

    public int GetMaxCapacity() {
      return _goodSamples.Max(sample => sample.TotalCapacity);
    }

    public bool WasAtFullOrZeroPreviously() {
      return GoodSamples[1].FillRate is > 0.999f or < 0.001f;
    }

    private void ReplaceMissingSamples(GoodSample goodSample) {
      for (var i = 1; i < _goodSamples.Count; i++) {
        if (_goodSamples[i].DayTimestamp < 0) {
          _goodSamples[i] = goodSample;
        } else {
          break;
        }
      }
    }

  }
}