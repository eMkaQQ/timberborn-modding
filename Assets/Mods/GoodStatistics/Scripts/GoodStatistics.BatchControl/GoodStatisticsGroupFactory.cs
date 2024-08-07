using GoodStatistics.Core;
using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.Goods;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsGroupFactory {

    private readonly GoodStatisticsBatchControlItemFactory _goodStatisticsBatchControlItemFactory;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly IGoodService _goodService;
    private readonly EventBus _eventBus;

    public GoodStatisticsGroupFactory(GoodStatisticsBatchControlItemFactory
                                          goodStatisticsBatchControlItemFactory,
                                      VisualElementLoader visualElementLoader,
                                      IGoodService goodService,
                                      EventBus eventBus) {
      _goodStatisticsBatchControlItemFactory = goodStatisticsBatchControlItemFactory;
      _visualElementLoader = visualElementLoader;
      _goodService = goodService;
      _eventBus = eventBus;
    }

    public GoodStatisticsGroup Create(GoodGroupSpecification groupSpecification,
                                      ResourceCountsRegistry resourceCountsRegistry) {
      var elementName = "GoodStatistics/GoodStatisticsGroup";
      var groupElement = _visualElementLoader.LoadVisualElement(elementName);
      groupElement.Q<Image>("Icon").sprite = groupSpecification.Icon;
      var items = CreateItems(groupSpecification.Id,
                              groupElement.Q<VisualElement>("Items"),
                              resourceCountsRegistry);
      var group = new GoodStatisticsGroup(_eventBus, groupElement, items);
      group.Initialize();
      return group;
    }

    private IEnumerable<GoodStatisticsBatchControlItem> CreateItems(string groupId,
                                                                    VisualElement parent,
                                                                    ResourceCountsRegistry
                                                                        resourceCountsRegistry) {
      foreach (var goodId in _goodService.GetGoodsForGroup(groupId)) {
        var resourceCountHistory = resourceCountsRegistry.GetGoodHistory(goodId);
        var item = _goodStatisticsBatchControlItemFactory.Create(resourceCountHistory);
        parent.Add(item.Root);
        yield return item;
      }
    }

  }
}