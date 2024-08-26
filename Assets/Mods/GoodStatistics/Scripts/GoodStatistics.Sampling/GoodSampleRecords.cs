using GoodStatistics.Settings;
using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Sampling {
  public class GoodSampleRecords {

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

    public int GetMaxCapacity() {
      return _goodSamples.Max(sample => sample.TotalCapacity);
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