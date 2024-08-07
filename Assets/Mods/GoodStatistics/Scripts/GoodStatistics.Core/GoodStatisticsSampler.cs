using System.Collections.Generic;
using Timberborn.GameDistricts;
using Timberborn.Goods;
using Timberborn.ResourceCountingSystem;
using Timberborn.SingletonSystem;

namespace GoodStatistics.Core {
  public class GoodStatisticsSampler {

    private readonly GlobalResourceCountsRegistry _globalResourceCountsRegistry;
    private readonly ResourceCountingService _resourceCountingService;
    private readonly DistrictContextService _districtContextService;
    private readonly IGoodService _goodService;
    private readonly EventBus _eventBus;
    private readonly List<DistrictResourceCountsRegistry> _districtResourceCountsRegistries = new();
    private float _nextSampleTime;

    public GoodStatisticsSampler(GlobalResourceCountsRegistry globalResourceCountsRegistry,
                                 ResourceCountingService resourceCountingService,
                                 DistrictContextService districtContextService,
                                 IGoodService goodService,
                                 EventBus eventBus) {
      _globalResourceCountsRegistry = globalResourceCountsRegistry;
      _resourceCountingService = resourceCountingService;
      _districtContextService = districtContextService;
      _goodService = goodService;
      _eventBus = eventBus;
    }

    public void AddDistrictRegistry(DistrictResourceCountsRegistry
                                        districtResourceCountsRegistry) {
      _districtResourceCountsRegistries.Add(districtResourceCountsRegistry);
    }

    public void RemoveDistrictRegistry(DistrictResourceCountsRegistry
                                           districtResourceCountsRegistry) {
      _districtResourceCountsRegistries.Remove(districtResourceCountsRegistry);
    }

    public void CollectGoodsSamples() {
      foreach (var goodId in _goodService.Goods) {
        CollectGoodSamples(goodId);
      }
      _eventBus.Post(new ResourceCountSampledEvent());
    }

    private void CollectGoodSamples(string goodId) {
      var sample = _resourceCountingService.GetGlobalResourceCount(goodId);
      _globalResourceCountsRegistry.ResourceCountsRegistry.AddSample(goodId, sample);
      CollectDistrictsSamples(goodId);
    }

    private void CollectDistrictsSamples(string goodId) {
      DistrictResourceCountsRegistry selectedRegistry = null;
      foreach (var districtRegistry in _districtResourceCountsRegistries) {
        if (districtRegistry.DistrictCenter != _districtContextService.SelectedDistrict) {
          _resourceCountingService.SwitchDistrict(districtRegistry.DistrictCenter);
          var districtSample = _resourceCountingService.GetDistrictResourceCount(goodId);
          districtRegistry.ResourceCountsRegistry.AddSample(goodId, districtSample);
        } else {
          selectedRegistry = districtRegistry;
        }
      }

      CollectSelectedDistrictSample(goodId, selectedRegistry);
    }

    private void CollectSelectedDistrictSample(string goodId,
                                               DistrictResourceCountsRegistry
                                                   selectedRegistry) {
      if (selectedRegistry) {
        _resourceCountingService.SwitchDistrict(_districtContextService.SelectedDistrict);
        var districtSample = _resourceCountingService.GetDistrictResourceCount(goodId);
        selectedRegistry.ResourceCountsRegistry.AddSample(goodId, districtSample);
      }
    }

  }
}