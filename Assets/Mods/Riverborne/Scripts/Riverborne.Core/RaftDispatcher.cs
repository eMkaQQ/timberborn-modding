using Timberborn.BaseComponentSystem;
using Timberborn.BlockingSystem;
using Timberborn.Common;
using Timberborn.InventorySystem;
using Timberborn.Persistence;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;
using Timberborn.WorldPersistence;
using UnityEngine;

namespace Riverborne.Core {
  public class RaftDispatcher : TickableComponent,
                                IPersistentEntity,
                                IAwakableComponent {

    private static readonly ComponentKey RaftDispatcherKey = new("Riverborne.RaftDispatcher");
    private static readonly PropertyKey<int> LastLaunchedDispatchIndexKey =
        new("LastLaunchedDispatchIndex");
    private static readonly PropertyKey<Raft> LastLaunchedRaftKey = new("LastLaunchedRaft");
    private readonly IDayNightCycle _dayNightCycle;
    private readonly ReferenceSerializer _referenceSerializer;
    private RaftDock _raftDock;
    private RaftSpawner _raftSpawner;
    private BlockableObject _blockableObject;
    private Inventory _inventory;
    private int _lastLaunchedDispatchIndex = -1;
    private Raft _lastLaunchedRaft;
    private GameObject _staticRaftObject;

    public RaftDispatcher(IDayNightCycle dayNightCycle,
                          ReferenceSerializer referenceSerializer) {
      _dayNightCycle = dayNightCycle;
      _referenceSerializer = referenceSerializer;
    }

    public void Awake() {
      _raftDock = GetComponent<RaftDock>();
      _raftSpawner = GetComponent<RaftSpawner>();
      _blockableObject = GetComponent<BlockableObject>();
      _inventory = GetComponent<RaftDockInventory>().Inventory;
      _staticRaftObject = GameObject.FindChild(GetComponent<RaftDockSpec>().StaticRaftName);
      _staticRaftObject.SetActive(true);
    }

    public override void Tick() {
      if (_blockableObject.IsUnblocked) {
        if (IsDropPointBlocked()) {
          _staticRaftObject.SetActive(false);
        } else {
          _staticRaftObject.SetActive(true);
          TryLaunchDispatch();
        }
      }
    }

    public void Save(IEntitySaver entitySaver) {
      if (_lastLaunchedRaft != null || _lastLaunchedDispatchIndex >= 0) {
        var raftDispatcher = entitySaver.GetComponent(RaftDispatcherKey);
        if (_lastLaunchedDispatchIndex >= 0) {
          raftDispatcher.Set(LastLaunchedDispatchIndexKey, _lastLaunchedDispatchIndex);
        }
        if (_lastLaunchedRaft != null) {
          raftDispatcher.Set(LastLaunchedRaftKey, _lastLaunchedRaft,
                             _referenceSerializer.Of<Raft>());
        }
      }
    }

    public void Load(IEntityLoader entityLoader) {
      if (entityLoader.TryGetComponent(RaftDispatcherKey, out var raftDispatcher)) {
        if (raftDispatcher.Has(LastLaunchedDispatchIndexKey)) {
          _lastLaunchedDispatchIndex = raftDispatcher.Get(LastLaunchedDispatchIndexKey);
        }
        if (raftDispatcher.Has(LastLaunchedRaftKey)) {
          raftDispatcher.GetObsoletable(LastLaunchedRaftKey, _referenceSerializer.Of<Raft>(),
                                        out _lastLaunchedRaft);
          _staticRaftObject.SetActive(false);
        }
      }
    }

    private bool IsDropPointBlocked() {
      if (!_lastLaunchedRaft) {
        _lastLaunchedRaft = null;
        return false;
      }
      var dropPoint = _raftSpawner.DropPoint.XZ();
      var raftPosition = _lastLaunchedRaft.Transform.position.XZ();

      if (Vector3.Distance(dropPoint, raftPosition) < 1f) {
        return true;
      }
      _lastLaunchedRaft = null;
      return false;
    }

    private void TryLaunchDispatch() {
      for (var index = _lastLaunchedDispatchIndex + 1;
           index < _raftDock.RaftDispatches.Count;
           index++) {
        if (TryLaunchDispatch(_raftDock.RaftDispatches[index])) {
          _lastLaunchedDispatchIndex = index;
          return;
        }
      }
      for (var index = 0;
           index <= _lastLaunchedDispatchIndex && index < _raftDock.RaftDispatches.Count;
           index++) {
        if (TryLaunchDispatch(_raftDock.RaftDispatches[index])) {
          _lastLaunchedDispatchIndex = index;
          return;
        }
      }
    }

    private bool TryLaunchDispatch(RaftDispatch dispatch) {
      var currentTime = _dayNightCycle.PartialDayNumber;
      if (!dispatch.IsPaused
          && currentTime - dispatch.LastDispatchTime >= dispatch.DayTimeInterval) {
        if (CanLaunch(dispatch)) {
          Launch(dispatch);
          return true;
        }
      }
      return false;
    }

    private bool CanLaunch(RaftDispatch dispatch) {
      foreach (var goodAmount in dispatch.Cargo) {
        if (_inventory.UnreservedAmountInStock(goodAmount.GoodId) < goodAmount.Amount) {
          return false;
        }
      }
      return true;
    }

    private void Launch(RaftDispatch dispatch) {
      foreach (var goodAmount in dispatch.Cargo) {
        _inventory.Take(goodAmount);
      }
      _lastLaunchedRaft = _raftSpawner.Spawn(dispatch);
      _staticRaftObject.SetActive(false);
      dispatch.UpdateLastDispatchTime(_dayNightCycle.PartialDayNumber);
    }

  }
}