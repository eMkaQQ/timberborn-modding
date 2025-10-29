using GoodStatistics.Analytics;
using GoodStatistics.Sampling;
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

    public GoodStatisticsGroup Create(GoodGroupSpec goodGroupSpec,
                                      GoodSamplesRegistry goodSamplesRegistry,
                                      GoodTrendsRegistry goodTrendsRegistry) {
      var elementName = "GoodStatistics/GoodStatisticsGroup";
      var groupElement = _visualElementLoader.LoadVisualElement(elementName);
      groupElement.Q<Image>("Icon").sprite = goodGroupSpec.Icon.Asset;
      var items = CreateItems(goodGroupSpec.Id,
                              groupElement.Q<VisualElement>("Items"),
                              goodSamplesRegistry,
                              goodTrendsRegistry);
      var group = new GoodStatisticsGroup(_eventBus, groupElement, items);
      group.Initialize();
      return group;
    }

    private IEnumerable<GoodStatisticsBatchControlItem> CreateItems(
        string groupId, VisualElement parent, GoodSamplesRegistry goodSamplesRegistry,
        GoodTrendsRegistry goodTrendsRegistry) {
      foreach (var goodId in _goodService.GetGoodsForGroup(groupId)) {
        var goodSampleRecords = goodSamplesRegistry.GetGoodSampleRecords(goodId);
        var goodTrend = goodTrendsRegistry.GetTrend(goodId);
        var item = _goodStatisticsBatchControlItemFactory.Create(goodSampleRecords, goodTrend);
        parent.Add(item.Root);
        yield return item;
      }
    }

  }
}