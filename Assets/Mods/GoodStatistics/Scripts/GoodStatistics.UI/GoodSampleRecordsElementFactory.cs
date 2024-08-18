using GoodStatistics.Sampling;
using GoodStatistics.Settings;
using System.Collections.Generic;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace GoodStatistics.UI {
  public class GoodSampleRecordsElementFactory {

    private readonly VisualElementLoader _visualElementLoader;

    public GoodSampleRecordsElementFactory(VisualElementLoader visualElementLoader) {
      _visualElementLoader = visualElementLoader;
    }

    public GoodSampleRecordsElement Create(GoodSampleRecords goodSampleRecords,
                                           VisualElement parent) {
      var element = new GoodSampleRecordsElement(goodSampleRecords,
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