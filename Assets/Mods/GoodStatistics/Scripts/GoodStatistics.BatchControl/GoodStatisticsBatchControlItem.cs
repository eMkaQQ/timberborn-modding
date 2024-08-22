using GoodStatistics.Analytics;
using GoodStatistics.AnalyticsUI;
using GoodStatistics.UI;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlItem {

    public VisualElement Root { get; }
    private readonly GoodSampleRecordsElement _goodSampleRecordsElement;
    private readonly GoodTrendElement _goodTrendElement;
    private readonly GoodTrend _goodTrend;

    public GoodStatisticsBatchControlItem(VisualElement root,
                                          GoodSampleRecordsElement goodSampleRecordsElement,
                                          GoodTrendElement goodTrendElement,
                                          GoodTrend goodTrend) {
      Root = root;
      _goodSampleRecordsElement = goodSampleRecordsElement;
      _goodTrendElement = goodTrendElement;
      _goodTrend = goodTrend;
    }

    public void Update() {
      _goodSampleRecordsElement.Update();
      _goodTrendElement.SetTrendType(_goodTrend.TrendType);
    }

  }
}