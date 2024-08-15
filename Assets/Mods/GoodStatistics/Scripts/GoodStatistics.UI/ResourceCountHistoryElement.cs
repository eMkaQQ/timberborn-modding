using GoodStatistics.Core;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace GoodStatistics.UI {
  public class ResourceCountHistoryElement {

    private readonly ResourceCountHistory _resourceCountHistory;
    private readonly ImmutableArray<ResourceCountElement> _resourceCountElements;

    public ResourceCountHistoryElement(ResourceCountHistory resourceCountHistory,
                                       IEnumerable<ResourceCountElement> resourceCountElements) {
      _resourceCountHistory = resourceCountHistory;
      _resourceCountElements = resourceCountElements.ToImmutableArray();
    }

    public void Update() {
      var resourceCounts = _resourceCountHistory.GoodSamples;
      var maxCapacity = (float) _resourceCountHistory.GetMaxCapacity();
      for (var index = 0; index < resourceCounts.Count; index++) {
        var elementIndex = resourceCounts.Count - index - 1;
        var fillRate = resourceCounts[index].TotalStock / maxCapacity;
        _resourceCountElements[elementIndex].SetFillRate(fillRate);
      }
    }

  }
}