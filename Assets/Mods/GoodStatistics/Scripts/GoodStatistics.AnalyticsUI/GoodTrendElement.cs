using GoodStatistics.Analytics;
using UnityEngine.UIElements;

namespace GoodStatistics.AnalyticsUI {
  public class GoodTrendElement {

    public VisualElement Root { get; }
    public string GoodId { get; }
    public GoodTrend GoodTrend { get; private set; }
    private readonly TrendIconProvider _trendIconProvider;
    private readonly Image _image;

    public GoodTrendElement(TrendIconProvider trendIconProvider,
                            VisualElement root,
                            string goodId,
                            Image image) {
      _trendIconProvider = trendIconProvider;
      Root = root;
      GoodId = goodId;
      _image = image;
    }

    public void Update(GoodTrend goodTrend) {
      GoodTrend = goodTrend;
      _image.sprite = _trendIconProvider.GetIcon(goodTrend.TrendType);
    }

  }
}