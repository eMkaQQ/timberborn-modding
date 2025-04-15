using System.Collections.Generic;
using Timberborn.GameDistricts;
using Timberborn.Goods;
using Timberborn.ResourceCountingSystem;
using Timberborn.SingletonSystem;
using Timberborn.TimeSystem;

namespace GoodStatistics.GoodSampling {
  public class GoodsSampler {

    private readonly GlobalGoodSamplesRegistry _globalGoodSamplesRegistry;
    private readonly ResourceCountingService _resourceCountingService;
    private readonly DistrictContextService _districtContextService;
    private readonly IGoodService _goodService;
    private readonly EventBus _eventBus;
    private readonly IDayNightCycle _dayNightCycle;
    private readonly List<DistrictGoodSamplesRegistry> _districtGoodSamplesRegistries = new();
    private float _nextSampleTime;

    public GoodsSampler(GlobalGoodSamplesRegistry globalGoodSamplesRegistry,
                        ResourceCountingService resourceCountingService,
                        DistrictContextService districtContextService,
                        IGoodService goodService,
                        EventBus eventBus,
                        IDayNightCycle dayNightCycle) {
      _globalGoodSamplesRegistry = globalGoodSamplesRegistry;
      _resourceCountingService = resourceCountingService;
      _districtContextService = districtContextService;
      _goodService = goodService;
      _eventBus = eventBus;
      _dayNightCycle = dayNightCycle;
    }

    public void AddDistrictRegistry(DistrictGoodSamplesRegistry
                                        districtGoodSamplesRegistry) {
      _districtGoodSamplesRegistries.Add(districtGoodSamplesRegistry);
    }

    public void RemoveDistrictRegistry(DistrictGoodSamplesRegistry
                                           districtGoodSamplesRegistry) {
      _districtGoodSamplesRegistries.Remove(districtGoodSamplesRegistry);
    }

    public void CollectGoodsSamples() {
      foreach (var goodId in _goodService.Goods) {
        CollectGoodSamples(goodId);
      }
      _eventBus.Post(new GoodsSampledEvent());
    }

    private void CollectGoodSamples(string goodId) {
      var resourceCount = _resourceCountingService.GetGlobalResourceCount(goodId);
      var sample = new GoodSample(resourceCount, _dayNightCycle.PartialDayNumber);
      _globalGoodSamplesRegistry.GoodSamplesRegistry.AddSample(goodId, sample);
      CollectDistrictsSamples(goodId);
    }

    private void CollectDistrictsSamples(string goodId) {
      DistrictGoodSamplesRegistry selectedRegistry = null;
      foreach (var districtRegistry in _districtGoodSamplesRegistries) {
        if (districtRegistry.DistrictCenter != _districtContextService.SelectedDistrict) {
          _resourceCountingService.SwitchDistrict(districtRegistry.DistrictCenter);
          var districtResource = _resourceCountingService.GetDistrictResourceCount(goodId);
          var districtSample = new GoodSample(districtResource, _dayNightCycle.PartialDayNumber);
          districtRegistry.GoodSamplesRegistry.AddSample(goodId, districtSample);
        } else {
          selectedRegistry = districtRegistry;
        }
      }

      CollectSelectedDistrictSample(goodId, selectedRegistry);
    }

    private void CollectSelectedDistrictSample(string goodId,
                                               DistrictGoodSamplesRegistry
                                                   selectedRegistry) {
      if (selectedRegistry) {
        _resourceCountingService.SwitchDistrict(_districtContextService.SelectedDistrict);
        var districtResource = _resourceCountingService.GetDistrictResourceCount(goodId);
        var districtSample = new GoodSample(districtResource, _dayNightCycle.PartialDayNumber);
        selectedRegistry.GoodSamplesRegistry.AddSample(goodId, districtSample);
      }
    }

  }
}