using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockingSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftPicker : BaseComponent,
                              IAwakableComponent,
                              IFinishedStateListener {

    private readonly EntityService _entityService;
    private readonly RaftPickingService _raftPickingService;
    private RaftDock _raftDock;
    private RaftDockInventory _raftDockInventory;
    private BlockableObject _blockableObject;
    private List<Vector3Int> _pickerCoordinates;

    public RaftPicker(EntityService entityService,
                      RaftPickingService raftPickingService) {
      _entityService = entityService;
      _raftPickingService = raftPickingService;
    }

    public ReadOnlyList<Vector3Int> PickerCoordinates => _pickerCoordinates.AsReadOnlyList();

    public void Awake() {
      _raftDock = GetComponent<RaftDock>();
      _raftDockInventory = GetComponent<RaftDockInventory>();
      _blockableObject = GetComponent<BlockableObject>();
    }

    public void OnEnterFinishedState() {
      var spec = GetComponent<RaftPickerSpec>();
      var blockObject = GetComponent<BlockObject>();
      _pickerCoordinates = new(spec.PickerCoordinates.Length);
      foreach (var specPickerCoordinate in spec.PickerCoordinates) {
        _pickerCoordinates.Add(blockObject.TransformCoordinates(specPickerCoordinate));
      }
      _raftPickingService.RegisterRaftPicker(this);
    }

    public void OnExitFinishedState() {
      _raftPickingService.UnregisterRaftPicker(this);
    }

    public void TryPickRaft(Raft raft) {
      if (raft.OriginDock != _raftDock && _blockableObject.IsUnblocked && HasCapacityFor(raft)) {
        foreach (var goodAmount in raft.Inventory.Stock) {
          _raftDockInventory.Inventory.Give(goodAmount);
        }
        _entityService.Delete(raft);
      }
    }

    private bool HasCapacityFor(Raft raft) {
      var inventory = _raftDockInventory.Inventory;
      foreach (var goodAmount in raft.Inventory.Stock) {
        if (!inventory.HasUnreservedCapacity(goodAmount)) {
          return false;
        }
      }
      return true;
    }

  }
}