using GoodStatistics.Core;
using System.Collections.Generic;
using System.Collections.Immutable;
using Timberborn.BatchControl;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsGroup : IBatchControlRowItem,
                                       IClearableBatchControlRowItem {

    public VisualElement Root { get; }
    private readonly EventBus _eventBus;
    private ImmutableArray<GoodStatisticsBatchControlItem> _goodStatisticsBatchControlItems;

    public GoodStatisticsGroup(EventBus eventBus,
                               VisualElement root,
                               IEnumerable<GoodStatisticsBatchControlItem>
                                   goodStatisticsBatchControlItems) {
      _eventBus = eventBus;
      Root = root;
      _goodStatisticsBatchControlItems = goodStatisticsBatchControlItems.ToImmutableArray();
    }

    public void Initialize() {
      _eventBus.Register(this);
    }

    public void ClearRowItem() {
      _eventBus.Unregister(this);
    }

    [OnEvent]
    public void OnResourceCountSampled(ResourceCountSampledEvent resourceCountSampledEvent) {
      foreach (var goodStatisticsBatchControlItem in _goodStatisticsBatchControlItems) {
        goodStatisticsBatchControlItem.Update();
      }
    }

  }
}