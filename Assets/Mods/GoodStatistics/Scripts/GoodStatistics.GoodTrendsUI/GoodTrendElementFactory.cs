using Timberborn.CoreUI;
using Timberborn.StockpilesUI;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.GoodTrendsUI {
  public class GoodTrendElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly TrendIconProvider _trendIconProvider;
    private readonly GoodStockpilesTooltipFactory _goodStockpilesTooltipFactory;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly GoodTrendTooltipFactory _goodTrendTooltipFactory;

    public GoodTrendElementFactory(VisualElementLoader visualElementLoader,
                                   TrendIconProvider trendIconProvider,
                                   GoodStockpilesTooltipFactory goodStockpilesTooltipFactory,
                                   ITooltipRegistrar tooltipRegistrar,
                                   GoodTrendTooltipFactory goodTrendTooltipFactory) {
      _visualElementLoader = visualElementLoader;
      _trendIconProvider = trendIconProvider;
      _goodStockpilesTooltipFactory = goodStockpilesTooltipFactory;
      _tooltipRegistrar = tooltipRegistrar;
      _goodTrendTooltipFactory = goodTrendTooltipFactory;
    }

    public GoodTrendElement Create(string goodId, VisualElement owner) {
      var root = _visualElementLoader.LoadVisualElement("GoodStatistics/GoodTrendElement");
      var goodTrendElement = new GoodTrendElement(_trendIconProvider, root,
                                                  goodId, root.Q<Image>());
      _tooltipRegistrar.Register(owner, () => CreateTooltip(goodTrendElement));
      return goodTrendElement;
    }

    private VisualElement CreateTooltip(GoodTrendElement goodTrendElement) {
      var tooltip = _goodStockpilesTooltipFactory.Create(goodTrendElement.GoodId);
      var label = _goodTrendTooltipFactory.Create(goodTrendElement.GoodTrend);
      tooltip.Add(label);
      return tooltip;
    }

  }
}