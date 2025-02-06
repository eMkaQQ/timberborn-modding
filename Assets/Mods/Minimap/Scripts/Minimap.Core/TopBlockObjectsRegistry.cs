using System;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.MapIndexSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace Minimap.Core {
  internal class TopBlockObjectsRegistry : ILoadableSingleton {

    private readonly BlockService _blockService;
    private readonly EventBus _eventBus;
    private readonly MapIndexService _mapIndexService;
    private readonly IMinimapBlockObjectRenderer _blockObjectRenderer;
    private TopBlock?[] _topBlocks;

    public TopBlockObjectsRegistry(BlockService blockService,
                                   EventBus eventBus,
                                   MapIndexService mapIndexService,
                                   IMinimapBlockObjectRenderer blockObjectRenderer) {
      _blockService = blockService;
      _eventBus = eventBus;
      _mapIndexService = mapIndexService;
      _blockObjectRenderer = blockObjectRenderer;
    }

    public void Load() {
      _topBlocks = new TopBlock?[_mapIndexService.MaxIndex];
      _eventBus.Register(this);
    }

    [OnEvent]
    public void OnEnteredFinishedState(EnteredFinishedStateEvent enteredFinishedStateEvent) {
      var blockObject = enteredFinishedStateEvent.BlockObject;
      CheckIfIsTopObject(blockObject);
    }

    [OnEvent]
    public void OnBlockObjectSet(BlockObjectSetEvent blockObjectSetEvent) {
      var blockObject = blockObjectSetEvent.BlockObject;
      if (blockObject.IsFinished) {
        CheckIfIsTopObject(blockObject);
      }
    }

    [OnEvent]
    public void OnBlockObjectUnset(BlockObjectUnsetEvent blockObjectUnsetEvent) {
      var blockObject = blockObjectUnsetEvent.BlockObject;
      foreach (var occupiedBlock in blockObject.PositionedBlocks.GetOccupiedBlocks()) {
        var coordinates = occupiedBlock.Coordinates.XY();
        var index = _mapIndexService.CellToIndex(coordinates);
        if (_topBlocks[index].HasValue && _topBlocks[index].Value.BlockObject == blockObject) {
          _topBlocks[index] = TryGetTopBlock(coordinates, out var topBlock) ? topBlock : null;
        }
      }
    }

    public bool TryGetTopBlock(int index, out TopBlock topBlock) {
      if (_topBlocks[index].HasValue) {
        topBlock = _topBlocks[index].Value;
        return true;
      }
      topBlock = default;
      return false;
    }

    private void CheckIfIsTopObject(BlockObject blockObject) {
      var renderer = GetRenderer(blockObject);
      foreach (var occupiedBlock in blockObject.PositionedBlocks.GetOccupiedBlocks()) {
        var index = _mapIndexService.CellToIndex(occupiedBlock.Coordinates.XY());
        var topBlock = new TopBlock(blockObject, occupiedBlock, renderer);
        if (!_topBlocks[index].HasValue
            || topBlock.Block.Coordinates.z > _topBlocks[index].Value.Block.Coordinates.z
            || topBlock.Block.Coordinates.z == _topBlocks[index].Value.Block.Coordinates.z
            && HasHigherOccupation(topBlock.Block, _topBlocks[index].Value.Block)) {
          _topBlocks[index] = topBlock;
        }
      }
    }

    private bool TryGetTopBlock(Vector2Int coordinates, out TopBlock topBlock) {
      for (var z = _blockService.Size.z; z >= 0; z--) {
        var blockCoordinates = new Vector3Int(coordinates.x, coordinates.y, z);
        foreach (var coordinateBlock in _blockService.GetObjectsAt(blockCoordinates)
                     .Select(blockObject => TopBlock.Create(blockObject, blockCoordinates,
                                                            GetRenderer(blockObject)))
                     .OrderByDescending(GetHighestOccupation)) {
          if (coordinateBlock.BlockObject.IsFinished) {
            topBlock = coordinateBlock;
            return true;
          }
        }
      }
      topBlock = default;
      return false;
    }

    private IMinimapBlockObjectRenderer GetRenderer(BlockObject blockObject) {
      var customRenderer = blockObject.GetComponentFast<IMinimapBlockObjectRenderer>();
      return customRenderer ?? _blockObjectRenderer;
    }

    private static bool HasHigherOccupation(Block newBlock, Block currentBlock) {
      return GetHighestOccupation(newBlock) > GetHighestOccupation(currentBlock);
    }

    private static int GetHighestOccupation(TopBlock topBlock) {
      return GetHighestOccupation(topBlock.Block);
    }

    private static int GetHighestOccupation(Block block) {
      if (block.Occupation.HasFlag(BlockOccupations.Top)) {
        return 6;
      }
      if (block.Occupation.HasFlag(BlockOccupations.Middle)) {
        return 5;
      }
      if (block.Occupation.HasFlag(BlockOccupations.Bottom)) {
        return 4;
      }
      if (block.Occupation.HasFlag(BlockOccupations.Corners)) {
        return 3;
      }
      if (block.Occupation.HasFlag(BlockOccupations.Path)) {
        return 2;
      }
      if (block.Occupation.HasFlag(BlockOccupations.Floor)) {
        return 1;
      }
      if (block.Occupation.HasFlag(BlockOccupations.None)) {
        return 0;
      }
      throw new ArgumentException($"Unknown block occupation: {block.Occupation}");
    }

  }
}