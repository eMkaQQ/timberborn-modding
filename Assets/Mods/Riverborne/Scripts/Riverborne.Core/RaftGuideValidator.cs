using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.Coordinates;
using Timberborn.RecoveredGoodSystem;
using Timberborn.TemplateSystem;
using Timberborn.TerrainSystem;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftGuideValidator : IBlockObjectValidator {

    private readonly IBlockService _blockService;
    private readonly ITerrainService _terrainService;

    public RaftGuideValidator(IBlockService blockService,
                              ITerrainService terrainService) {
      _blockService = blockService;
      _terrainService = terrainService;
    }

    public bool IsValid(BlockObject blockObject, out string errorMessage) {
      errorMessage = string.Empty;
      if (blockObject.HasComponent<RecoveredGoodStack>()) {
        return true;
      }
      foreach (var foundationCoordinate in
               blockObject.PositionedBlocks.GetFoundationCoordinates()) {
        var belowCoordinate = foundationCoordinate.Below();
        if (!_terrainService.Underground(belowCoordinate)) {
          if (!ValidateObjectBelow(blockObject, belowCoordinate)) {
            return false;
          }
        }
      }
      return true;
    }

    private bool ValidateObjectBelow(BlockObject blockObject, Vector3Int belowCoordinate) {
      var raftGuideBelow =
          _blockService.GetTopObjectComponentAt<RaftGuideSpec>(belowCoordinate);
      if (raftGuideBelow != null) {
        if (blockObject.HasComponent<RaftGuideSpec>()) {
          var blockObjectBelow =
              _blockService.GetTopObjectComponentAt<BlockObject>(belowCoordinate);
          return ValidateRaftGuide(blockObject, blockObjectBelow, raftGuideBelow);
        }
        return false;
      }
      return true;
    }

    private static bool ValidateRaftGuide(BlockObject blockObject, BlockObject blockObjectBelow,
                                          RaftGuideSpec raftGuideBelow) {
      if (blockObject.GetComponent<TemplateSpec>()
          .IsNamedExactly(blockObjectBelow.GetComponent<TemplateSpec>().TemplateName)) {
        return blockObject.Placement.Orientation == blockObjectBelow.Placement.Orientation
               || raftGuideBelow.FlippedOrientationSupports
               && blockObject.Placement.Orientation.Flip()
               == blockObjectBelow.Placement.Orientation;
      }
      return false;
    }

  }
}