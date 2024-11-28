using Minimap.Core;
using Minimap.Settings;
using UnityEngine;

namespace Minimap.Renderers {
  internal class BlockObjectMinimapRenderer : IMinimapBlockObjectRenderer {

    private readonly MinimapColorSettings _minimapColorSettings;

    public BlockObjectMinimapRenderer(MinimapColorSettings minimapColorSettings) {
      _minimapColorSettings = minimapColorSettings;
    }

    public Color GetColor() {
      return _minimapColorSettings.BlockObjectColor.Color;
    }

  }
}