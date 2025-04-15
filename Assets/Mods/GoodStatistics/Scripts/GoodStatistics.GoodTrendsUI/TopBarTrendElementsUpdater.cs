using GoodStatistics.GoodSampling;
using GoodStatistics.GoodTrends;
using GoodStatistics.Settings;
using System.Collections.Generic;
using Timberborn.GameDistricts;
using Timberborn.SingletonSystem;
using Timberborn.UILayoutSystem;

namespace GoodStatistics.GoodTrendsUI {
  internal class TopBarTrendElementsUpdater : ILoadableSingleton {

    private readonly EventBus _eventBus;
    private readonly DistrictContextService _districtContextService;
    private readonly GlobalGoodTrendsRegistry _globalGoodTrendsRegistry;
    private readonly TopBarPanelTrendDecorator _topBarPanelTrendDecorator;
    private readonly GoodStatisticsSettings _goodStatisticsSettings;
    private readonly List<GoodTrendElement> _goodTrendElements = new();

    public TopBarTrendElementsUpdater(EventBus eventBus,
                                      DistrictContextService districtContextService,
                                      GlobalGoodTrendsRegistry globalGoodTrendsRegistry,
                                      TopBarPanelTrendDecorator topBarPanelTrendDecorator,
                                      GoodStatisticsSettings goodStatisticsSettings) {
      _eventBus = eventBus;
      _districtContextService = districtContextService;
      _globalGoodTrendsRegistry = globalGoodTrendsRegistry;
      _topBarPanelTrendDecorator = topBarPanelTrendDecorator;
      _goodStatisticsSettings = goodStatisticsSettings;
    }

    public void Load() {
      if (_goodStatisticsSettings.ShowTrendsOnTopBar.Value) {
        _eventBus.Register(this);
      }
    }

    [OnEvent]
    public void OnShowPrimaryUI(ShowPrimaryUIEvent showPrimaryUIEvent) {
      _goodTrendElements.AddRange(_topBarPanelTrendDecorator.CreateTrendElements());
      UpdateTrendElements();
    }

    [OnEvent]
    public void OnDistrictSelected(DistrictSelectedEvent districtSelectedEvent) {
      UpdateTrendElements();
    }

    [OnEvent]
    public void OnDistrictUnselected(DistrictUnselectedEvent districtUnselectedEvent) {
      UpdateTrendElements();
    }

    [OnEvent]
    public void OnGoodsSampled(GoodsSampledEvent goodsSampledEvent) {
      UpdateTrendElements();
    }

    private void UpdateTrendElements() {
      var trendRegistry = _districtContextService.SelectedDistrict
          ? _districtContextService.SelectedDistrict.GetComponentFast<DistrictGoodTrendsRegistry>()
              .GoodTrendsRegistry
          : _globalGoodTrendsRegistry.GoodTrendsRegistry;
      foreach (var goodTrendElement in _goodTrendElements) {
        goodTrendElement.Update(trendRegistry.GetTrend(goodTrendElement.GoodId));
      }
    }

  }
}