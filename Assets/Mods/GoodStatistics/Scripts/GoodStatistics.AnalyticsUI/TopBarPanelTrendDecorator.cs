using System.Collections.Generic;
using Timberborn.TopBarSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.AnalyticsUI {
  internal class TopBarPanelTrendDecorator {

    private readonly GoodTrendElementFactory _goodTrendElementFactory;
    private readonly TopBarPanel _topBarPanel;

    public TopBarPanelTrendDecorator(GoodTrendElementFactory goodTrendElementFactory,
                                     TopBarPanel topBarPanel) {
      _goodTrendElementFactory = goodTrendElementFactory;
      _topBarPanel = topBarPanel;
    }

    public IEnumerable<GoodTrendElement> CreateTrendElements() {
      foreach (var counter in _topBarPanel._counters) {
        if (counter is TopBarCounterRow topBarCounter) {
          yield return CreateTrendElement(topBarCounter);
        } else if (counter is ExtendableTopBarCounter extendableTopBarCounter) {
          foreach (var counterRow in extendableTopBarCounter._counterRows) {
            yield return CreateTrendElement(counterRow);
          }
        } else {
          throw new($"Unknown counter type: {counter.GetType().FullName}");
        }
      }
    }

    private GoodTrendElement CreateTrendElement(TopBarCounterRow topBarCounterRow) {
      var root = topBarCounterRow._root;
      var wrapper = root.Q<VisualElement>("CounterWrapper");
      var owner = wrapper ?? root;
      var trendElement = _goodTrendElementFactory.Create(topBarCounterRow._goodId, owner);
      root.Q<VisualElement>("Icon").Add(trendElement.Root);
      return trendElement;
    }

  }
}