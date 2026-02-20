using Timberborn.Goods;
using Timberborn.InventoryNeedSystem;
using Timberborn.InventorySystem;
using Timberborn.TemplateInstantiation;

namespace Riverborne.Core {
  internal class RaftDockInventoryInitializer :
      IDedicatedDecoratorInitializer<RaftDockInventory, Inventory> {

    private static readonly string InventoryComponentName = "Riverborne.RaftDock";
    private static readonly int DistrictCrossingCapacity = 70;
    private readonly IGoodService _goodService;
    private readonly InventoryNeedBehaviorInitializer _inventoryNeedBehaviorInitializer;
    private readonly InventoryInitializerFactory _inventoryInitializerFactory;

    public RaftDockInventoryInitializer(IGoodService goodService,
                                        InventoryNeedBehaviorInitializer
                                            inventoryNeedBehaviorInitializer,
                                        InventoryInitializerFactory
                                            inventoryInitializerFactory) {
      _goodService = goodService;
      _inventoryNeedBehaviorInitializer = inventoryNeedBehaviorInitializer;
      _inventoryInitializerFactory = inventoryInitializerFactory;
    }

    public void Initialize(RaftDockInventory subject, Inventory decorator) {
      var inventoryInitializer =
          _inventoryInitializerFactory.CreateWithUnlimitedCapacity(decorator,
                                                                   InventoryComponentName);
      AllowEveryGood(inventoryInitializer);
      inventoryInitializer.HasPublicOutput();
      inventoryInitializer.SetIgnorableCapacity();
      inventoryInitializer.Initialize();
      subject.InitializeInventory(decorator);
      _inventoryNeedBehaviorInitializer.AddNeedBehavior(decorator);
    }

    private void AllowEveryGood(InventoryInitializer inventoryInitializer) {
      foreach (var goodId in _goodService.Goods) {
        var storableGood = StorableGood.CreateAsTakeable(goodId);
        var storableGoodAmount = new StorableGoodAmount(storableGood, DistrictCrossingCapacity);
        inventoryInitializer.AddAllowedGood(storableGoodAmount);
      }
    }

  }
}