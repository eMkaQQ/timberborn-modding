using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.InventorySystem;

namespace Riverborne.Core {
  public class RaftDockInventory : BaseComponent,
                                   IAwakableComponent,
                                   IFinishedStateListener {

    public Inventory Inventory { get; private set; }
    private RaftDock _raftDock;

    public void Awake() {
      _raftDock = GetComponent<RaftDock>();
    }

    public void OnEnterFinishedState() {
      Inventory.Enable();
      UpdateInputOutputGoods();
      _raftDock.RaftDispatchesChanged += OnRaftDispatchesChanged;
    }

    public void OnExitFinishedState() {
      Inventory.Disable();
      _raftDock.RaftDispatchesChanged -= OnRaftDispatchesChanged;
    }

    public void InitializeInventory(Inventory inventory) {
      Asserts.FieldIsNull(this, Inventory, nameof(Inventory));
      Inventory = inventory;
    }

    private void OnRaftDispatchesChanged(object sender, System.EventArgs e) {
      UpdateInputOutputGoods();
    }

    private void UpdateInputOutputGoods() {
      var inputGoods = new HashSet<string>();
      foreach (var raftDispatch in _raftDock.RaftDispatches) {
        foreach (var goodAmount in raftDispatch.Cargo) {
          inputGoods.Add(goodAmount.GoodId);
        }
      }
      Inventory._allowedGoods._inputGoods.Clear();
      Inventory._allowedGoods._outputGoods.Clear();
      foreach (var allowedGoodsStorableGood in Inventory._allowedGoods._storableGoods) {
        if (inputGoods.Contains(allowedGoodsStorableGood.StorableGood.GoodId)) {
          Inventory._allowedGoods._inputGoods.Add(allowedGoodsStorableGood.StorableGood.GoodId);
        } else {
          Inventory._allowedGoods._outputGoods.Add(allowedGoodsStorableGood.StorableGood.GoodId);
        }
      }
    }

  }
}