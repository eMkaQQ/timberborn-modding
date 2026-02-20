using System.Collections.Generic;
using Timberborn.MapIndexSystem;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftPickingService {

    private readonly MapIndexService _mapIndexService;
    private readonly Dictionary<int, RaftPicker> _raftPickers = new();

    public RaftPickingService(MapIndexService mapIndexService) {
      _mapIndexService = mapIndexService;
    }

    public void RegisterRaftPicker(RaftPicker raftPicker) {
      foreach (var pickerCoordinate in raftPicker.PickerCoordinates) {
        var coordinateIndex = _mapIndexService.CoordinatesToIndex3D(pickerCoordinate);
        _raftPickers.Add(coordinateIndex, raftPicker);
      }
    }

    public void UnregisterRaftPicker(RaftPicker raftPicker) {
      foreach (var pickerCoordinate in raftPicker.PickerCoordinates) {
        var coordinateIndex = _mapIndexService.CoordinatesToIndex3D(pickerCoordinate);
        _raftPickers.Remove(coordinateIndex);
      }
    }

    public RaftPicker GetRaftPicker(Vector3Int coordinate) {
      var coordinateIndex = _mapIndexService.CoordinatesToIndex3D(coordinate);
      return _raftPickers.GetValueOrDefault(coordinateIndex);
    }

  }
}