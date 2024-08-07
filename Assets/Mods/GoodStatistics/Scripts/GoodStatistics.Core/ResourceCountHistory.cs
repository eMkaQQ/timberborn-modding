using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Core {
  public class ResourceCountHistory {

    public static readonly int MaxSamples = 20;
    public string GoodId { get; }
    private readonly List<ResourceCount> _resourceCounts;

    private ResourceCountHistory(string goodId,
                                 List<ResourceCount> resourceCounts) {
      GoodId = goodId;
      _resourceCounts = resourceCounts;
    }

    public static ResourceCountHistory CreateNew(string goodId) {
      var resourceCounts = Enumerable.Repeat(ResourceCount.Create(0, 0, 0, 0), MaxSamples).ToList();
      return new(goodId, resourceCounts);
    }

    public static ResourceCountHistory CreateFromSave(string goodId,
                                                      List<ResourceCount> resourceCounts) {
      return new(goodId, resourceCounts);
    }

    public ReadOnlyList<ResourceCount> ResourceCounts => new(_resourceCounts);

    public void Add(ResourceCount resourceCount) {
      _resourceCounts.Insert(0, resourceCount);
      if (_resourceCounts.Count > MaxSamples) {
        _resourceCounts.RemoveAt(_resourceCounts.Count - 1);
      }
    }

    public int GetMaxCapacity() {
      return _resourceCounts.Max(resourceCount => resourceCount.InputOutputCapacity);
    }

  }
}