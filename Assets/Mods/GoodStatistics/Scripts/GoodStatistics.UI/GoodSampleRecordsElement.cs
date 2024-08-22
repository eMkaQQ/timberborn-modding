using GoodStatistics.Sampling;
using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

namespace GoodStatistics.UI {
  public class GoodSampleRecordsElement {

    private readonly GoodSampleRecords _goodSampleRecords;
    private readonly ImmutableArray<ResourceCountElement> _resourceCountElements;

    public GoodSampleRecordsElement(GoodSampleRecords goodSampleRecords,
                                    IEnumerable<ResourceCountElement> resourceCountElements) {
      _goodSampleRecords = goodSampleRecords;
      _resourceCountElements = resourceCountElements.ToImmutableArray();
    }

    public void Update() {
      var goodSamples = _goodSampleRecords.GoodSamples;
      var maxCapacity = (float) _goodSampleRecords.GetMaxCapacity();
      for (var index = 0; index < goodSamples.Count; index++) {
        var elementIndex = goodSamples.Count - index - 1;
        var fillRate = GetFillRate(goodSamples[index].TotalStock, maxCapacity);
        _resourceCountElements[elementIndex].SetFillRate(fillRate);
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