using GoodStatistics.Analytics;
using GoodStatistics.Trends;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace GoodStatistics.TrendsUI {
  public class GoodTrendTooltipFactory {

    private static readonly string GrowingLocKey = "eMka.GoodStatistics.GrowingDaysLeft";
    private static readonly string DepletionLocKey = "eMka.GoodStatistics.DepletionDaysLeft";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly GoodTrendDaysLeftDescriber _goodTrendDaysLeftDescriber;
    private readonly ILoc _loc;

    public GoodTrendTooltipFactory(VisualElementLoader visualElementLoader,
                                   GoodTrendDaysLeftDescriber goodTrendDaysLeftDescriber,
                                   ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _goodTrendDaysLeftDescriber = goodTrendDaysLeftDescriber;
      _loc = loc;
    }

    public VisualElement Create(GoodTrend goodTrend) {
      if (goodTrend.TrendType == TrendType.Stable) {
        return null;
      }
      var tooltip = _visualElementLoader.LoadVisualElement("GoodStatistics/GoodTrendTooltip");
      tooltip.Q<Label>("TooltipLabel").text = GetEstimationText(goodTrend);
      return tooltip;
    }

    private string GetEstimationText(GoodTrend goodTrend) {
      var daysLeft = _goodTrendDaysLeftDescriber.GetDaysLeftText(goodTrend.DaysLeft);
      if (goodTrend.TrendType.IsGrowing()) {
        return _loc.T(GrowingLocKey, daysLeft);
      }
      return _loc.T(DepletionLocKey, daysLeft);
    }

  }
}