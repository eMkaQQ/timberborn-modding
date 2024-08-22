using GoodStatistics.Analytics;
using GoodStatistics.AnalyticsUI;
using GoodStatistics.Sampling;
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
    private readonly GoodSampleRecordsElementFactory _goodSampleRecordsElementFactory;
    private readonly GoodTrendElementFactory _goodTrendElementFactory;

    public GoodStatisticsBatchControlItemFactory(ITooltipRegistrar tooltipRegistrar,
                                                 VisualElementLoader visualElementLoader,
                                                 GoodDescriber goodDescriber,
                                                 GoodSampleRecordsElementFactory
                                                     goodSampleRecordsElementFactory,
                                                 GoodTrendElementFactory goodTrendElementFactory) {
      _tooltipRegistrar = tooltipRegistrar;
      _visualElementLoader = visualElementLoader;
      _goodDescriber = goodDescriber;
      _goodSampleRecordsElementFactory = goodSampleRecordsElementFactory;
      _goodTrendElementFactory = goodTrendElementFactory;
    }

    public GoodStatisticsBatchControlItem Create(GoodSampleRecords goodSampleRecords,
                                                 GoodTrend goodTrend) {
      var elementName = "GoodStatistics/GoodStatisticsBatchControlItem";
      var item = _visualElementLoader.LoadVisualElement(elementName);

      var goodIcon = item.Q<Image>("GoodIcon");
      var describedGood = _goodDescriber.GetDescribedGood(goodSampleRecords.GoodId);
      goodIcon.sprite = describedGood.Icon;
      var trendElement = _goodTrendElementFactory.Create(goodSampleRecords.GoodId);
      goodIcon.Add(trendElement.Root);

      var records = _goodSampleRecordsElementFactory.Create(
          goodSampleRecords, item.Q<VisualElement>("GoodSampleRecordsWrapper"));

      return new(item, records, trendElement, goodTrend);
    }

  }
}