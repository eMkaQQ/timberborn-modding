using GoodStatistics.Core;
using GoodStatistics.UI;
using Timberborn.CoreUI;
using Timberborn.GoodsUI;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlItemFactory {

    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly GoodDescriber _goodDescriber;
    private readonly ResourceCountHistoryElementFactory _resourceCountHistoryElementFactory;

    public GoodStatisticsBatchControlItemFactory(ITooltipRegistrar tooltipRegistrar,
                                                 VisualElementLoader visualElementLoader,
                                                 GoodDescriber goodDescriber,
                                                 ResourceCountHistoryElementFactory
                                                     resourceCountHistoryElementFactory) {
      _tooltipRegistrar = tooltipRegistrar;
      _visualElementLoader = visualElementLoader;
      _goodDescriber = goodDescriber;
      _resourceCountHistoryElementFactory = resourceCountHistoryElementFactory;
    }

    public GoodStatisticsBatchControlItem Create(ResourceCountHistory resourceCountHistory) {
      var elementName = "GoodStatistics/GoodStatisticsBatchControlItem";
      var item = _visualElementLoader.LoadVisualElement(elementName);

      var goodIcon = item.Q<Image>("GoodIcon");
      var describedGood = _goodDescriber.GetDescribedGood(resourceCountHistory.GoodId);
      goodIcon.sprite = describedGood.Icon;
      var history = _resourceCountHistoryElementFactory.Create(
          resourceCountHistory, item.Q<VisualElement>("ResourceCountHistoryWrapper"));

      return new(item, history);
    }

  }
}