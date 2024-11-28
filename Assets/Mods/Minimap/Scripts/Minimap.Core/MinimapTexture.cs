using Minimap.Settings;
using Timberborn.MapStateSystem;
using Timberborn.SingletonSystem;
using Timberborn.TextureOperations;
using UnityEngine;

namespace Minimap.Core {
  public class MinimapTexture : ILoadableSingleton,
                                IUnloadableSingleton {

    public Texture2D Texture { get; private set; }
    private readonly MapSize _mapSize;
    private readonly TextureFactory _textureFactory;
    private readonly MinimapSettings _minimapSettings;

    public MinimapTexture(MapSize mapSize,
                          TextureFactory textureFactory,
                          MinimapSettings minimapSettings) {
      _mapSize = mapSize;
      _textureFactory = textureFactory;
      _minimapSettings = minimapSettings;
    }

    public bool MinimapEnabled => Texture;

    public void Load() {
      if (EnableMinimap()) {
        var textureSettings = new TextureSettings.Builder()
            .SetTextureFormat(TextureFormat.RGB24)
            .SetSize(_mapSize.TerrainSize.x, _mapSize.TerrainSize.y)
            .SetSpritePreset()
            .SetFilterMode(FilterMode.Point)
            .SetAnisoLevel(0)
            .SetName("MinimapTexture")
            .Build();
        Texture = _textureFactory.CreateTexture(textureSettings);
      }
    }

    public void Unload() {
      if (Texture) {
        Object.Destroy(Texture);
      }
    }

    private bool EnableMinimap() {
      return _minimapSettings.DisableSize.Value < _mapSize.TerrainSize.x
             || _minimapSettings.DisableSize.Value < _mapSize.TerrainSize.y;
    }

  }
}