using GoodStatistics.GoodSampling;
using GoodStatistics.Settings;
using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.TimeSystem;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.GoodUI {
  public class GoodSampleRecordsElementFactory {

    private static readonly string HoursShortLocKey = "Time.HoursShort";
    private static readonly string SampleTimeLocKey = "eMka.GoodStatistics.GoodSampleTime";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly ILoc _loc;
    private readonly IDayNightCycle _dayNightCycle;

    public GoodSampleRecordsElementFactory(VisualElementLoader visualElementLoader,
                                           ITooltipRegistrar tooltipRegistrar,
                                           ILoc loc,
                                           IDayNightCycle dayNightCycle) {
      _visualElementLoader = visualElementLoader;
      _tooltipRegistrar = tooltipRegistrar;
      _loc = loc;
      _dayNightCycle = dayNightCycle;
    }

    public GoodSampleRecordsElement Create(GoodSampleRecords goodSampleRecords,
                                           VisualElement parent) {
      var element = new GoodSampleRecordsElement(goodSampleRecords,
                                                 CreateGoodSampleElements(parent));
      element.Update();
      return element;
    }

    private IEnumerable<GoodSampleElement> CreateGoodSampleElements(VisualElement parent) {
      for (var index = 0; index < GoodStatisticsConstants.MaxSamples; index++) {
        var root = _visualElementLoader.LoadVisualElement("GoodStatistics/GoodSampleElement");
        var sampleElement = new GoodSampleElement(root.Q<VisualElement>("Fill"));
        _tooltipRegistrar.Register(
            root, () => TooltipContent.CreateInstant(() => CreateGoodSampleTooltip(sampleElement)));
        parent.Add(root);
        yield return sampleElement;
      }
    }

    private VisualElement CreateGoodSampleTooltip(GoodSampleElement goodSampleElement) {
      if (goodSampleElement.GoodSample.DayTimestamp >= 0) {
        var root =
            _visualElementLoader.LoadVisualElement("GoodStatistics/GoodSampleElementTooltip");
        var resourceCount = goodSampleElement.GoodSample.ResourceCount;
        root.Q<Label>("WorkplaceAmount").text = $"{resourceCount.OutputStock}";
        root.Q<Label>("StockpileAmount").text =
            $"{resourceCount.InputOutputStock} / {resourceCount.InputOutputCapacity}";
        root.Q<Label>("SampleTime").text = GetSampleTimeText(goodSampleElement.GoodSample);
        return root;
      }
      return null;
    }

    private string GetSampleTimeText(GoodSample goodSample) {
      var timeDiff = _dayNightCycle.PartialDayNumber - goodSample.DayTimestamp;
      var secondsDiff = timeDiff * _dayNightCycle.ConfiguredDayLengthInSeconds;
      var hoursDiff = (int) _dayNightCycle.SecondsToHours(secondsDiff);
      return _loc.T(SampleTimeLocKey, _loc.T(HoursShortLocKey, hoursDiff.ToString()));
    }

  }
}