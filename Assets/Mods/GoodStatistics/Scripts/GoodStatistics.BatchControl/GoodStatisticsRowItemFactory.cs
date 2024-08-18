using GoodStatistics.Sampling;
using Timberborn.BatchControl;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.Goods;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsRowItemFactory {

    private readonly GoodStatisticsGroupFactory _goodStatisticsGroupFactory;
    private readonly GoodsGroupSpecificationService _goodsGroupSpecificationService;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly BatchControlDistrict _batchControlDistrict;
    private readonly GlobalGoodSamplesRegistry _globalGoodSamplesRegistry;

    public GoodStatisticsRowItemFactory(GoodStatisticsGroupFactory goodStatisticsGroupFactory,
                                        GoodsGroupSpecificationService
                                            goodsGroupSpecificationService,
                                        VisualElementLoader visualElementLoader,
                                        BatchControlDistrict batchControlDistrict,
                                        GlobalGoodSamplesRegistry
                                            globalGoodSamplesRegistry) {
      _goodStatisticsGroupFactory = goodStatisticsGroupFactory;
      _goodsGroupSpecificationService = goodsGroupSpecificationService;
      _visualElementLoader = visualElementLoader;
      _batchControlDistrict = batchControlDistrict;
      _globalGoodSamplesRegistry = globalGoodSamplesRegistry;
    }

    public BatchControlRow Create(DistrictGoodSamplesRegistry districtGoodSamplesRegistry) {
      var elementName = "Game/BatchControl/DistributionSettingsRowItem";
      var root = _visualElementLoader.LoadVisualElement(elementName);
      return new(root, districtGoodSamplesRegistry.GetComponentFast<EntityComponent>(),
                 () => _batchControlDistrict.SelectedDistrict,
                 CreateGoodGroups(districtGoodSamplesRegistry.GoodSamplesRegistry));
    }

    public BatchControlRow CreateGlobal() {
      var elementName = "Game/BatchControl/DistributionSettingsRowItem";
      var root = _visualElementLoader.LoadVisualElement(elementName);
      return new(root, null, () => !_batchControlDistrict.SelectedDistrict,
                 CreateGoodGroups(_globalGoodSamplesRegistry.GoodSamplesRegistry));
    }

    private ReadOnlyList<GoodGroupSpecification> GoodGroupSpecifications =>
        _goodsGroupSpecificationService.GoodGroupSpecifications;

    private IBatchControlRowItem[] CreateGoodGroups(GoodSamplesRegistry goodSamplesRegistry) {
      var result = new IBatchControlRowItem[GoodGroupSpecifications.Count];
      for (var i = 0; i < GoodGroupSpecifications.Count; i++) {
        var groupSpecification = GoodGroupSpecifications[i];
        result[i] = _goodStatisticsGroupFactory.Create(groupSpecification, goodSamplesRegistry);
      }
      return result;
    }

  }
}