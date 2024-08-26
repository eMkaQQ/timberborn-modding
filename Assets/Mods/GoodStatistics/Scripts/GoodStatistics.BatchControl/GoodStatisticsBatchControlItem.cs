using GoodStatistics.Analytics;
using GoodStatistics.AnalyticsUI;
using GoodStatistics.UI;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlItem {

    public VisualElement Root { get; }
    private readonly GoodTrendDaysLeftDescriber _goodTrendDaysLeftDescriber;
    private readonly GoodSampleRecordsElement _goodSampleRecordsElement;
    private readonly GoodTrendElement _goodTrendElement;
    private readonly GoodTrend _goodTrend;
    private readonly Label _daysLeftLabel;

    public GoodStatisticsBatchControlItem(GoodTrendDaysLeftDescriber goodTrendDaysLeftDescriber,
                                          VisualElement root,
                                          GoodSampleRecordsElement goodSampleRecordsElement,
                                          GoodTrendElement goodTrendElement,
                                          GoodTrend goodTrend,
                                          Label daysLeftLabel) {
      _goodTrendDaysLeftDescriber = goodTrendDaysLeftDescriber;
      Root = root;
      _goodSampleRecordsElement = goodSampleRecordsElement;
      _goodTrendElement = goodTrendElement;
      _goodTrend = goodTrend;
      _daysLeftLabel = daysLeftLabel;
    }

    public void Update() {
      _goodSampleRecordsElement.Update();
      _goodTrendElement.Update(_goodTrend);
      _daysLeftLabel.text = _goodTrendDaysLeftDescriber.GetDaysLeftText(_goodTrend.DaysLeft);
    }

  }
}