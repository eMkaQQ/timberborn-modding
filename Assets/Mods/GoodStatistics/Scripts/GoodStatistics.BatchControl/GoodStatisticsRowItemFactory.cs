using GoodStatistics.Analytics;
using GoodStatistics.Sampling;
using Timberborn.BatchControl;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.Goods;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsRowItemFactory {

    private readonly GoodStatisticsGroupFactory _goodStatisticsGroupFactory;
    private readonly GoodsGroupSpecService _goodsGroupSpecService;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly BatchControlDistrict _batchControlDistrict;
    private readonly GlobalGoodSamplesRegistry _globalGoodSamplesRegistry;
    private readonly GlobalGoodTrendsRegistry _globalGoodTrendsRegistry;

    public GoodStatisticsRowItemFactory(GoodStatisticsGroupFactory goodStatisticsGroupFactory,
                                        GoodsGroupSpecService goodsGroupSpecService,
                                        VisualElementLoader visualElementLoader,
                                        BatchControlDistrict batchControlDistrict,
                                        GlobalGoodSamplesRegistry globalGoodSamplesRegistry,
                                        GlobalGoodTrendsRegistry globalGoodTrendsRegistry) {
      _goodStatisticsGroupFactory = goodStatisticsGroupFactory;
      _goodsGroupSpecService = goodsGroupSpecService;
      _visualElementLoader = visualElementLoader;
      _batchControlDistrict = batchControlDistrict;
      _globalGoodSamplesRegistry = globalGoodSamplesRegistry;
      _globalGoodTrendsRegistry = globalGoodTrendsRegistry;
    }

    public BatchControlRow Create(DistrictGoodSamplesRegistry districtGoodSamplesRegistry) {
      var elementName = "Game/BatchControl/DistributionSettingsRowItem";
      var root = _visualElementLoader.LoadVisualElement(elementName);
      var trendRegistry =
          districtGoodSamplesRegistry.GetComponent<DistrictGoodTrendsRegistry>()
              .GoodTrendsRegistry;
      return new(root, districtGoodSamplesRegistry.GetComponent<EntityComponent>(),
                 () => _batchControlDistrict.SelectedDistrict,
                 CreateGoodGroups(districtGoodSamplesRegistry.GoodSamplesRegistry, trendRegistry));
    }

    public BatchControlRow CreateGlobal() {
      var elementName = "Game/BatchControl/DistributionSettingsRowItem";
      var root = _visualElementLoader.LoadVisualElement(elementName);
      return new(root, null, () => !_batchControlDistrict.SelectedDistrict,
                 CreateGoodGroups(_globalGoodSamplesRegistry.GoodSamplesRegistry,
                                  _globalGoodTrendsRegistry.GoodTrendsRegistry));
    }

    private ReadOnlyList<GoodGroupSpec> GoodGroupSpecifications =>
        _goodsGroupSpecService.GoodGroupSpecs;

    private IBatchControlRowItem[] CreateGoodGroups(GoodSamplesRegistry goodSamplesRegistry,
                                                    GoodTrendsRegistry goodTrendsRegistry) {
      var result = new IBatchControlRowItem[GoodGroupSpecifications.Count];
      for (var i = 0; i < GoodGroupSpecifications.Count; i++) {
        var groupSpecification = GoodGroupSpecifications[i];
        result[i] = _goodStatisticsGroupFactory.Create(groupSpecification, goodSamplesRegistry,
                                                       goodTrendsRegistry);
      }
      return result;
    }

  }
}