using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace GoodStatistics.AnalyticsUI {
  public class GoodTrendElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly TrendIconProvider _trendIconProvider;

    public GoodTrendElementFactory(VisualElementLoader visualElementLoader,
                                   TrendIconProvider trendIconProvider) {
      _visualElementLoader = visualElementLoader;
      _trendIconProvider = trendIconProvider;
    }

    public GoodTrendElement Create(string goodId) {
      var root = _visualElementLoader.LoadVisualElement("GoodStatistics/GoodTrendElement");
      return new(_trendIconProvider, root, goodId, root.Q<Image>());
    }

  }
}