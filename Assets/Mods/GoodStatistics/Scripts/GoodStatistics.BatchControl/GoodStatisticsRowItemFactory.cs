using GoodStatistics.Core;
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
    private readonly GlobalResourceCountsRegistry _globalResourceCountsRegistry;

    public GoodStatisticsRowItemFactory(GoodStatisticsGroupFactory goodStatisticsGroupFactory,
                                        GoodsGroupSpecificationService
                                            goodsGroupSpecificationService,
                                        VisualElementLoader visualElementLoader,
                                        BatchControlDistrict batchControlDistrict,
                                        GlobalResourceCountsRegistry
                                            globalResourceCountsRegistry) {
      _goodStatisticsGroupFactory = goodStatisticsGroupFactory;
      _goodsGroupSpecificationService = goodsGroupSpecificationService;
      _visualElementLoader = visualElementLoader;
      _batchControlDistrict = batchControlDistrict;
      _globalResourceCountsRegistry = globalResourceCountsRegistry;
    }

    public BatchControlRow Create(DistrictResourceCountsRegistry districtResourceCountsRegistry) {
      var elementName = "Game/BatchControl/DistributionSettingsRowItem";
      var root = _visualElementLoader.LoadVisualElement(elementName);
      return new(root, districtResourceCountsRegistry.GetComponentFast<EntityComponent>(),
                 () => _batchControlDistrict.SelectedDistrict,
                 CreateResourceGroups(districtResourceCountsRegistry.ResourceCountsRegistry));
    }

    public BatchControlRow CreateGlobal() {
      var elementName = "Game/BatchControl/DistributionSettingsRowItem";
      var root = _visualElementLoader.LoadVisualElement(elementName);
      return new(root, null, () => !_batchControlDistrict.SelectedDistrict,
                 CreateResourceGroups(_globalResourceCountsRegistry.ResourceCountsRegistry));
    }

    private ReadOnlyList<GoodGroupSpecification> GoodGroupSpecifications =>
        _goodsGroupSpecificationService.GoodGroupSpecifications;

    private IBatchControlRowItem[] CreateResourceGroups(ResourceCountsRegistry
                                                            resourceCountsRegistry) {
      var result = new IBatchControlRowItem[GoodGroupSpecifications.Count];
      for (var i = 0; i < GoodGroupSpecifications.Count; i++) {
        var groupSpecification = GoodGroupSpecifications[i];
        result[i] = _goodStatisticsGroupFactory.Create(groupSpecification, resourceCountsRegistry);
      }
      return result;
    }

  }
}