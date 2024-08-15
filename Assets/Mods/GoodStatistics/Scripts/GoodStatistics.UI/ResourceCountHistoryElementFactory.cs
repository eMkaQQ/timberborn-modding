using GoodStatistics.Core;
using GoodStatistics.Settings;
using System.Collections.Generic;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace GoodStatistics.UI {
  public class ResourceCountHistoryElementFactory {

    private readonly VisualElementLoader _visualElementLoader;

    public ResourceCountHistoryElementFactory(VisualElementLoader visualElementLoader) {
      _visualElementLoader = visualElementLoader;
    }

    public ResourceCountHistoryElement Create(ResourceCountHistory resourceCountHistory,
                                              VisualElement parent) {
      var element = new ResourceCountHistoryElement(resourceCountHistory,
                                                    CreateResourceCountElements(parent));
      element.Update();
      return element;
    }

    private IEnumerable<ResourceCountElement> CreateResourceCountElements(VisualElement parent) {
      for (var index = 0; index < GoodStatisticsConstants.MaxSamples; index++) {
        var root = _visualElementLoader.LoadVisualElement("GoodStatistics/ResourceProgressBar");
        parent.Add(root);
        yield return new(root.Q<VisualElement>("Fill"));
      }
    }

  }
}