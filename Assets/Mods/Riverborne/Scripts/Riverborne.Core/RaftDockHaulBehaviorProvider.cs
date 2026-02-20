using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockingSystem;
using Timberborn.Emptying;
using Timberborn.Hauling;
using Timberborn.InventorySystem;

namespace Riverborne.Core {
  internal class RaftDockHaulBehaviorProvider : BaseComponent,
                                                IAwakableComponent,
                                                IHaulBehaviorProvider {

    private readonly InventoryFillCalculator _inventoryFillCalculator;
    private EmptyOutputWorkplaceBehavior _emptyOutputWorkplaceBehavior;
    private BlockableObject _blockableObject;
    private Inventory _inventory;

    public RaftDockHaulBehaviorProvider(InventoryFillCalculator
                                            inventoryFillCalculator) {
      _inventoryFillCalculator = inventoryFillCalculator;
    }

    public void Awake() {
      _emptyOutputWorkplaceBehavior = GetComponent<EmptyOutputWorkplaceBehavior>();
      _blockableObject = GetComponent<BlockableObject>();
      _inventory = GetComponent<RaftDockInventory>().Inventory;
    }

    public void GetWeightedBehaviors(IList<WeightedBehavior> weightedBehaviors) {
      if (_inventory.Enabled && _blockableObject.IsUnblocked) {
        var outputWeight = _inventoryFillCalculator.GetInStockOutputFillPercentage(_inventory);
        weightedBehaviors.Add(new(outputWeight, _emptyOutputWorkplaceBehavior));
      }
    }

  }
}