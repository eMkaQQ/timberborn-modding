using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.GameDistricts;
using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.Core {
  public class DistrictResourceCountsRegistry : BaseComponent,
                                                IFinishedStateListener,
                                                IPersistentEntity {

    private static readonly ComponentKey DistrictResourceCountsRegistryKey =
        new("DistrictResourceCountsRegistry");
    private static readonly PropertyKey<ResourceCountsRegistry> ResourceCountsRegistryKey =
        new("ResourceCountsRegistry");
    public ResourceCountsRegistry ResourceCountsRegistry { get; private set; }
    public DistrictCenter DistrictCenter { get; private set; }
    private GoodStatisticsSampler _goodStatisticsSampler;
    private ResourceCountsRegistrySerializer _resourceCountsRegistrySerializer;
    private IGoodService _goodService;

    [Inject]
    public void InjectDependencies(GoodStatisticsSampler goodStatisticsSampler,
                                   ResourceCountsRegistrySerializer
                                       resourceCountsRegistrySerializer,
                                   IGoodService goodService) {
      _goodStatisticsSampler = goodStatisticsSampler;
      _resourceCountsRegistrySerializer = resourceCountsRegistrySerializer;
      _goodService = goodService;
    }

    public void Awake() {
      DistrictCenter = GetComponentFast<DistrictCenter>();
    }

    public void Save(IEntitySaver entitySaver) {
      var districtResourceCountsRegistry =
          entitySaver.GetComponent(DistrictResourceCountsRegistryKey);
      districtResourceCountsRegistry.Set(ResourceCountsRegistryKey, ResourceCountsRegistry,
                                         _resourceCountsRegistrySerializer);
    }

    public void Load(IEntityLoader entityLoader) {
      if (entityLoader.HasComponent(DistrictResourceCountsRegistryKey)) {
        var districtResourceCountsRegistry =
            entityLoader.GetComponent(DistrictResourceCountsRegistryKey);
        ResourceCountsRegistry = districtResourceCountsRegistry.Get(
            ResourceCountsRegistryKey, _resourceCountsRegistrySerializer);
      }
    }

    public void OnEnterFinishedState() {
      ResourceCountsRegistry ??= ResourceCountsRegistry.CreateNew(_goodService.Goods);
      _goodStatisticsSampler.AddDistrictRegistry(this);
    }

    public void OnExitFinishedState() {
      _goodStatisticsSampler.RemoveDistrictRegistry(this);
    }

  }
}