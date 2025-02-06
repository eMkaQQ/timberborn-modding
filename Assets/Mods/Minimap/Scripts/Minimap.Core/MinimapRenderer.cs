using Minimap.Settings;
using Timberborn.MapIndexSystem;
using Timberborn.SingletonSystem;
using Timberborn.SoilMoistureSystem;
using Timberborn.TerrainSystem;
using Timberborn.TextureOperations;
using Timberborn.WaterSystem;
using UnityEngine;

namespace Minimap.Core {
  internal class MinimapRenderer : IUpdatableSingleton,
                                   IPostLoadableSingleton {

    private readonly MinimapTexture _minimapTexture;
    private readonly IThreadSafeWaterMap _threadSafeWaterMap;
    private readonly ISoilMoistureService _soilMoistureService;
    private readonly MapIndexService _mapIndexService;
    private readonly ITerrainService _terrainService;
    private readonly MinimapSettings _minimapSettings;
    private readonly MinimapColorSettings _minimapColorSettings;
    private readonly EventBus _eventBus;
    private readonly TopBlockObjectsRegistry _topBlockObjectsRegistry;
    private readonly TextureFactory _textureFactory;
    private readonly IThreadSafeColumnTerrainMap _threadSafeColumnTerrainMap;
    private Vector3Int _mapSize;
    private Color32[] _chunkPixels;
    private Texture2D _chunkTexture;
    private bool _isEnabled;
    private int _xChunkSize;
    private int _yChunkSize;
    private int _lastChunkX;
    private int _lastChunkY;

    public MinimapRenderer(MinimapTexture minimapTexture,
                           IThreadSafeWaterMap threadSafeWaterMap,
                           ISoilMoistureService soilMoistureService,
                           MapIndexService mapIndexService,
                           ITerrainService terrainService,
                           MinimapSettings minimapSettings,
                           MinimapColorSettings minimapColorSettings,
                           EventBus eventBus,
                           TopBlockObjectsRegistry topBlockObjectsRegistry,
                           TextureFactory textureFactory,
                           IThreadSafeColumnTerrainMap threadSafeColumnTerrainMap) {
      _minimapTexture = minimapTexture;
      _threadSafeWaterMap = threadSafeWaterMap;
      _soilMoistureService = soilMoistureService;
      _mapIndexService = mapIndexService;
      _terrainService = terrainService;
      _minimapSettings = minimapSettings;
      _minimapColorSettings = minimapColorSettings;
      _eventBus = eventBus;
      _topBlockObjectsRegistry = topBlockObjectsRegistry;
      _textureFactory = textureFactory;
      _threadSafeColumnTerrainMap = threadSafeColumnTerrainMap;
    }

    public void PostLoad() {
      if (_minimapTexture.MinimapEnabled) {
        _mapSize = _mapIndexService.TerrainSize;
        _eventBus.Register(this);
        ResetChunkValues();
        RenderFull();
      }
    }

    public void UpdateSingleton() {
      if (_minimapTexture.MinimapEnabled) {
        RenderChunk();
      }
    }

    [OnEvent]
    public void OnMinimapSettingsChanged(MinimapSettingsChangedEvent minimapSettingsChangedEvent) {
      if (minimapSettingsChangedEvent.ChunkSizeChanged) {
        ResetChunkValues();
      }
      RenderFull();
    }

    private void ResetChunkValues() {
      var settingsSize = _minimapSettings.ChunkSize.Value;
      _xChunkSize = Mathf.Min(settingsSize, _mapSize.x);
      _yChunkSize = Mathf.Min(settingsSize, _mapSize.y);
      _chunkPixels = new Color32[_xChunkSize * _yChunkSize];

      var textureSettings = new TextureSettings.Builder()
          .SetTextureFormat(TextureFormat.RGB24)
          .SetSize(_xChunkSize, _yChunkSize)
          .SetSpritePreset()
          .SetFilterMode(FilterMode.Point)
          .SetAnisoLevel(0)
          .SetName("MinimapChunkTexture")
          .Build();

      if (_chunkTexture) {
        Object.Destroy(_chunkTexture);
      }
      _chunkTexture = _textureFactory.CreateTexture(textureSettings);
    }

    private void RenderFull() {
      var minimapPixels = new Color32[_mapSize.x * _mapSize.y];
      for (var y = 0; y < _mapSize.y; y++) {
        for (var x = 0; x < _mapSize.x; x++) {
          minimapPixels[y * _mapSize.x + x] = GetColor(new(x, y));
        }
      }
      _minimapTexture.Texture.SetPixels32(minimapPixels);
      _minimapTexture.Texture.Apply(false);
    }

    private void RenderChunk() {
      var chunkX = _lastChunkX;
      var chunkY = _lastChunkY;
      var j = 0;
      for (var y = chunkY; y < chunkY + _yChunkSize && y < _mapSize.y; y++) {
        var i = 0;
        for (var x = chunkX; x < chunkX + _xChunkSize && x < _mapSize.x; x++) {
          _chunkPixels[j * _xChunkSize + i] = GetColor(new(x, y));
          i++;
        }
        j++;
      }
      _lastChunkY += _yChunkSize;
      if (_lastChunkY >= _mapSize.y) {
        _lastChunkY = 0;
        _lastChunkX += _xChunkSize;
        if (_lastChunkX >= _mapSize.x) {
          _lastChunkX = 0;
        }
      }

      var width = _xChunkSize + chunkX > _mapSize.x ? _mapSize.x - chunkX : _xChunkSize;
      var height = _yChunkSize + chunkY > _mapSize.y ? _mapSize.y - chunkY : _yChunkSize;

      _chunkTexture.SetPixels32(_chunkPixels);
      _chunkTexture.Apply(false);
      Graphics.CopyTexture(_chunkTexture, 0, 0, 0, 0, width, height,
                           _minimapTexture.Texture, 0, 0, chunkX, chunkY);
    }

    private Color GetColor(Vector2Int coordinates) {
      var index2D = _mapIndexService.CellToIndex(coordinates);
      var waterHeight = GetWaterHeight(index2D, out var waterIndex3D);
      var terrainHeight = GetTerrainHeight(index2D, out var terrainIndex3D);
      if (_topBlockObjectsRegistry.TryGetTopBlock(index2D, out var topBlock)
          && (!waterHeight.HasValue || topBlock.Block.Coordinates.z >= waterHeight)
          && topBlock.Block.Coordinates.z >= terrainHeight) {
        return topBlock.Renderer.GetColor();
      }
      if (waterHeight > terrainHeight) {
        return GetWaterColor(waterIndex3D);
      }
      return GetTerrainColor(terrainHeight, terrainIndex3D);
    }

    private int? GetWaterHeight(int index2D, out int waterIndex3D) {
      if (_threadSafeWaterMap.TryGetTopWateredColumn(int.MaxValue, index2D, out waterIndex3D)) {
        return _threadSafeWaterMap.CeiledWaterHeight(waterIndex3D);
      }
      return null;
    }

    private int GetTerrainHeight(int index2D, out int terrainIndex3D) {
      var columnCount = _threadSafeColumnTerrainMap.GetColumnCount(index2D);
      terrainIndex3D = index2D + (columnCount - 1) * _mapIndexService.VerticalStride;
      return _threadSafeColumnTerrainMap.GetColumnCeiling(terrainIndex3D);
    }

    private Color GetWaterColor(int index3D) {
      var contamination = _threadSafeWaterMap.ColumnContamination(index3D);
      var contaminationGradient = contamination < 0.05f ? contamination * 10 :
          contamination < 0.5f ? 0.5f + (contamination - 0.05f) * 2.22f : 1;
      var waterDepth = _threadSafeWaterMap.WaterDepth(index3D);
      var waterProportion = waterDepth / 10;
      var shallowWaterColor = _minimapColorSettings.ShallowWaterColor.Color;
      var deepWaterColor = _minimapColorSettings.DeepWaterColor.Color;
      var waterColor = Color.Lerp(shallowWaterColor, deepWaterColor, waterProportion);
      var shallowBadwaterColor =
          _minimapColorSettings.ShallowBadwaterColor.Color;
      var deepBadwaterColor = _minimapColorSettings.DeepBadwaterColor.Color;
      var badwaterColor = Color.Lerp(shallowBadwaterColor, deepBadwaterColor, waterProportion);
      return Color.Lerp(waterColor, badwaterColor, contaminationGradient);
    }

    private Color GetTerrainColor(float height, int index3D) {
      var heightProportion = height / _terrainService.Size.z;
      if (_soilMoistureService.SoilMoisture(index3D) > 0) {
        var lowestGrassColor = _minimapColorSettings.LowestGrassColor.Color;
        var highestGrassColor = _minimapColorSettings.HighestGrassColor.Color;
        return Color.Lerp(lowestGrassColor, highestGrassColor, heightProportion);
      }
      var lowestTerrainColor = _minimapColorSettings.LowestTerrainColor.Color;
      var highestTerrainColor = _minimapColorSettings.HighestTerrainColor.Color;
      return Color.Lerp(lowestTerrainColor, highestTerrainColor, heightProportion);
    }

  }
}