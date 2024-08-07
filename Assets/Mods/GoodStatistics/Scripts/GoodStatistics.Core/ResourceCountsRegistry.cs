using System.Collections.Generic;
using Timberborn.Common;
using Timberborn.ResourceCountingSystem;

namespace GoodStatistics.Core {
  public class ResourceCountsRegistry {

    private readonly Dictionary<string, ResourceCountHistory> _resourceCountHistoriesMap = new();
    private readonly List<ResourceCountHistory> _resourceCountHistories = new();

    public static ResourceCountsRegistry CreateNew(IReadOnlyList<string> goodIds) {
      var resourceCountsRegistry = new ResourceCountsRegistry();
      foreach (var goodId in goodIds) {
        resourceCountsRegistry.AddMissingGood(goodId);
      }
      return resourceCountsRegistry;
    }

    public static ResourceCountsRegistry CreateFromSave(List<ResourceCountHistory>
                                                            resourceCountHistories) {
      var resourceCountsRegistry = new ResourceCountsRegistry();
      foreach (var resourceCountHistory in resourceCountHistories) {
        resourceCountsRegistry._resourceCountHistoriesMap[resourceCountHistory.GoodId] =
            resourceCountHistory;
        resourceCountsRegistry._resourceCountHistories.Add(resourceCountHistory);
      }
      return resourceCountsRegistry;
    }

    public ReadOnlyList<ResourceCountHistory> ResourceCountHistories =>
        new(_resourceCountHistories);

    public void AddSample(string goodId, ResourceCount resourceCount) {
      _resourceCountHistoriesMap[goodId].Add(resourceCount);
    }

    public bool HasGood(string goodId) {
      return _resourceCountHistoriesMap.ContainsKey(goodId);
    }

    public ResourceCountHistory GetGoodHistory(string goodId) {
      return _resourceCountHistoriesMap[goodId];
    }

    public void AddMissingGood(string goodId) {
      var resourceCountHistory = ResourceCountHistory.CreateNew(goodId);
      _resourceCountHistoriesMap[goodId] = resourceCountHistory;
      _resourceCountHistories.Add(resourceCountHistory);
    }

  }
}