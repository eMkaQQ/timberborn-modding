using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;
using Timberborn.GameDistricts;
using Timberborn.Goods;
using Timberborn.Persistence;
using Timberborn.WorldPersistence;

namespace GoodStatistics.GoodSampling {
  public class DistrictGoodSamplesRegistry : BaseComponent,
                                             IInitializableEntity,
                                             IFinishedStateListener,
                                             IPersistentEntity {

    private static readonly ComponentKey DistrictGoodSamplesRegistryKey =
        new("DistrictGoodSamplesRegistry");
    private static readonly PropertyKey<GoodSamplesRegistry> GoodSamplesRegistryKey =
        new("GoodSamplesRegistry");
    public GoodSamplesRegistry GoodSamplesRegistry { get; private set; }
    public DistrictCenter DistrictCenter { get; private set; }
    private GoodsSampler _goodsSampler;
    private GoodSamplesRegistrySerializer _goodSamplesRegistrySerializer;
    private IGoodService _goodService;

    [Inject]
    public void InjectDependencies(GoodsSampler goodsSampler,
                                   GoodSamplesRegistrySerializer goodSamplesRegistrySerializer,
                                   IGoodService goodService) {
      _goodsSampler = goodsSampler;
      _goodSamplesRegistrySerializer = goodSamplesRegistrySerializer;
      _goodService = goodService;
    }

    public void Awake() {
      DistrictCenter = GetComponentFast<DistrictCenter>();
    }

    public void InitializeEntity() {
      GoodSamplesRegistry ??= GoodSamplesRegistry.CreateNew(_goodService.Goods);
    }

    public void Save(IEntitySaver entitySaver) {
      var districtGoodSamplesRegistry = entitySaver.GetComponent(DistrictGoodSamplesRegistryKey);
      districtGoodSamplesRegistry.Set(GoodSamplesRegistryKey, GoodSamplesRegistry,
                                      _goodSamplesRegistrySerializer);
    }

    public void Load(IEntityLoader entityLoader) {
      if (entityLoader.TryGetComponent(DistrictGoodSamplesRegistryKey,
                                       out var districtGoodSamplesRegistry)) {
        GoodSamplesRegistry = districtGoodSamplesRegistry.Get(GoodSamplesRegistryKey,
                                                              _goodSamplesRegistrySerializer);
      }
    }

    public void OnEnterFinishedState() {
      _goodsSampler.AddDistrictRegistry(this);
    }

    public void OnExitFinishedState() {
      _goodsSampler.RemoveDistrictRegistry(this);
    }

  }
}