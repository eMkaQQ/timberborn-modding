using System;
using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.Persistence;
using Timberborn.WorldPersistence;

namespace Riverborne.Core {
  public class RaftDock : BaseComponent,
                          IPersistentEntity {

    private static readonly ComponentKey RaftDockKey = new("Riverborne.RaftDock");
    private static readonly ListKey<RaftDispatch> RaftDispatchesKey = new("RaftDispatches");
    public event EventHandler RaftDispatchesChanged;
    private readonly RaftDispatchSerializer _raftDispatchSerializer;
    private readonly List<RaftDispatch> _raftDispatches = new();

    public RaftDock(RaftDispatchSerializer raftDispatchSerializer) {
      _raftDispatchSerializer = raftDispatchSerializer;
    }

    public ReadOnlyList<RaftDispatch> RaftDispatches => _raftDispatches.AsReadOnlyList();

    public void Save(IEntitySaver entitySaver) {
      var raftDock = entitySaver.GetComponent(RaftDockKey);
      raftDock.Set(RaftDispatchesKey, _raftDispatches, _raftDispatchSerializer);
    }

    public void Load(IEntityLoader entityLoader) {
      var raftDock = entityLoader.GetComponent(RaftDockKey);
      _raftDispatches.AddRange(raftDock.Get(RaftDispatchesKey, _raftDispatchSerializer));
    }

    public void AddRaftDispatch(RaftDispatch dispatch) {
      _raftDispatches.Add(dispatch);
      NotifyRaftDispatchesChanged();
    }

    public void RemoveRaftDispatch(RaftDispatch dispatch) {
      _raftDispatches.Remove(dispatch);
      NotifyRaftDispatchesChanged();
    }

    public void ReplaceRaftDispatch(RaftDispatch oldDispatch, RaftDispatch newDispatch) {
      var index = _raftDispatches.IndexOf(oldDispatch);
      if (index >= 0) {
        _raftDispatches[index] = newDispatch;
        NotifyRaftDispatchesChanged();
      } else {
        throw new KeyNotFoundException("Old Dispatch not found in raft dock dispatches.");
      }
    }

    public string GetNextDispatchName(string baseName) {
      var highestNumber = 1;
      foreach (var dispatch in _raftDispatches) {
        if (dispatch.Name.StartsWith(baseName)) {
          var suffix = dispatch.Name[baseName.Length..];
          if (int.TryParse(suffix, out var number)) {
            if (number >= highestNumber) {
              highestNumber = number + 1;
            }
          }
        }
      }
      return $"{baseName}{highestNumber}";
    }

    private void NotifyRaftDispatchesChanged() {
      RaftDispatchesChanged?.Invoke(this, EventArgs.Empty);
    }

  }
}