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

    private static readonly string LargeIconClass = "good-trend-element--large";
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly GoodDescriber _goodDescriber;
    private readonly GoodSampleRecordsElementFactory _goodSampleRecordsElementFactory;
    private readonly GoodTrendElementFactory _goodTrendElementFactory;
    private readonly GoodTrendDaysLeftDescriber _goodTrendDaysLeftDescriber;
    private readonly GoodTrendTooltipFactory _goodTrendTooltipFactory;

    public GoodStatisticsBatchControlItemFactory(ITooltipRegistrar tooltipRegistrar,
                                                 VisualElementLoader visualElementLoader,
                                                 GoodDescriber goodDescriber,
                                                 GoodSampleRecordsElementFactory
                                                     goodSampleRecordsElementFactory,
                                                 GoodTrendElementFactory goodTrendElementFactory,
                                                 GoodTrendDaysLeftDescriber
                                                     goodTrendDaysLeftDescriber,
                                                 GoodTrendTooltipFactory goodTrendTooltipFactory) {
      _tooltipRegistrar = tooltipRegistrar;
      _visualElementLoader = visualElementLoader;
      _goodDescriber = goodDescriber;
      _goodSampleRecordsElementFactory = goodSampleRecordsElementFactory;
      _goodTrendElementFactory = goodTrendElementFactory;
      _goodTrendDaysLeftDescriber = goodTrendDaysLeftDescriber;
      _goodTrendTooltipFactory = goodTrendTooltipFactory;
    }

    public GoodStatisticsBatchControlItem Create(GoodSampleRecords goodSampleRecords,
                                                 GoodTrend goodTrend) {
      var elementName = "GoodStatistics/GoodStatisticsBatchControlItem";
      var item = _visualElementLoader.LoadVisualElement(elementName);

      var goodIcon = item.Q<Image>("GoodIcon");
      var describedGood = _goodDescriber.GetDescribedGood(goodSampleRecords.GoodId);
      goodIcon.sprite = describedGood.Icon;
      var trendElement = _goodTrendElementFactory.Create(goodSampleRecords.GoodId, goodIcon);
      trendElement.Root.AddToClassList(LargeIconClass);
      goodIcon.Add(trendElement.Root);
      var records = _goodSampleRecordsElementFactory.Create(
          goodSampleRecords, item.Q<VisualElement>("GoodSampleRecordsWrapper"));
      var daysLeftLabel = item.Q<Label>("DaysLeftLabel");
      _tooltipRegistrar.Register(daysLeftLabel, () => _goodTrendTooltipFactory.Create(goodTrend));
      return new(_goodTrendDaysLeftDescriber, item, records,
                 trendElement, goodTrend, daysLeftLabel);
    }

  }
}