using Timberborn.BlockSystem;
using UnityEngine;

namespace Minimap.Core {
  internal readonly struct TopBlock {

    public BlockObject BlockObject { get; }
    public Block Block { get; }
    public IMinimapBlockObjectRenderer Renderer { get; }

    public TopBlock(BlockObject blockObject,
                    Block block,
                    IMinimapBlockObjectRenderer renderer) {
      BlockObject = blockObject;
      Block = block;
      Renderer = renderer;
    }

    public static TopBlock Create(BlockObject blockObject, Vector3Int coordinates,
                                  IMinimapBlockObjectRenderer renderer) {
      return new(blockObject, blockObject.PositionedBlocks.GetBlock(coordinates), renderer);
    }

  }
}