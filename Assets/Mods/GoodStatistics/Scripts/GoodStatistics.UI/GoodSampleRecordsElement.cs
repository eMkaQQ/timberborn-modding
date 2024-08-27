using GoodStatistics.Sampling;
using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

namespace GoodStatistics.UI {
  public class GoodSampleRecordsElement {

    private readonly GoodSampleRecords _goodSampleRecords;
    private readonly ImmutableArray<GoodSampleElement> _goodSampleElements;

    public GoodSampleRecordsElement(GoodSampleRecords goodSampleRecords,
                                    IEnumerable<GoodSampleElement> goodSampleElements) {
      _goodSampleRecords = goodSampleRecords;
      _goodSampleElements = goodSampleElements.ToImmutableArray();
    }

    public void Update() {
      var goodSamples = _goodSampleRecords.GoodSamples;
      var maxCapacity = (float) _goodSampleRecords.GetMaxCapacity();
      for (var index = 0; index < goodSamples.Count; index++) {
        var elementIndex = goodSamples.Count - index - 1;
        var fillRate = GetFillRate(goodSamples[index].TotalStock, maxCapacity);
        _goodSampleElements[elementIndex].Update(goodSamples[index], fillRate);
      }
    }

    private static float GetFillRate(int amount, float capacity) {
      if (amount == 0) {
        return 0;
      }
      if (capacity == 0) {
        return 1;
      }
      return Mathf.Clamp01(amount / capacity);
    }

  }
}