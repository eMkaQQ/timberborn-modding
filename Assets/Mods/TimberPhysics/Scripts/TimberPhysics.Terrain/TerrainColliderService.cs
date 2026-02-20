using JetBrains.Annotations;
using System.Collections.Generic;
using Timberborn.Coordinates;
using Timberborn.MapIndexSystem;
using Timberborn.MapStateSystem;
using Timberborn.RootProviders;
using Timberborn.SingletonSystem;
using Timberborn.TerrainSystem;
using TimberPhysics.Layers;
using UnityEngine;

namespace TimberPhysics.Terrain {
  public class TerrainColliderService : ILoadableSingleton {

    [UsedImplicitly]
    public static readonly string TerrainLayerName = "Terrain";
    private readonly RootObjectProvider _rootObjectProvider;
    private readonly ColumnTerrainMap _columnTerrainMap;
    private readonly MapSize _mapSize;
    private readonly MapIndexService _mapIndexService;
    private readonly PhysicsLayerRegistry _physicsLayerRegistry;
    private readonly ITerrainService _terrainService;
    private GameObject _rootObject;
    private readonly Dictionary<int, BoxCollider> _boxColliders = new();

    public TerrainColliderService(RootObjectProvider rootObjectProvider,
                                  ColumnTerrainMap columnTerrainMap,
                                  MapSize mapSize,
                                  MapIndexService mapIndexService,
                                  PhysicsLayerRegistry physicsLayerRegistry,
                                  ITerrainService terrainService) {
      _rootObjectProvider = rootObjectProvider;
      _columnTerrainMap = columnTerrainMap;
      _mapSize = mapSize;
      _mapIndexService = mapIndexService;
      _physicsLayerRegistry = physicsLayerRegistry;
      _terrainService = terrainService;
    }

    public void Load() {
      _rootObject = _rootObjectProvider.CreateRootObject("TerrainColliders");
      _rootObject.isStatic = true;
      _physicsLayerRegistry.AssignGameObjectToLayer(_rootObject, TerrainLayerName);
      SpawnColliders();
      _terrainService.PreTerrainHeightChanged += OnPreTerrainHeightChanged;
      _terrainService.TerrainHeightChanged += OnTerrainHeightChanged;
    }

    internal void ToggleColliders() {
      _rootObject.SetActive(!_rootObject.activeSelf);
    }

    private void SpawnColliders() {
      for (var y = 0; y < _mapSize.TerrainSize.y; y++) {
        for (var x = 0; x < _mapSize.TerrainSize.x; x++) {
          SpawnCollidersXY(new(x, y));
        }
      }
    }

    private void OnPreTerrainHeightChanged(object sender,
                                           TerrainHeightChangeEventArgs
                                               terrainHeightChangeEventArgs) {
      RemoveCollidersXY(terrainHeightChangeEventArgs.Change.Coordinates);
    }

    private void OnTerrainHeightChanged(object sender,
                                        TerrainHeightChangeEventArgs terrainHeightChangeEventArgs) {
      SpawnCollidersXY(terrainHeightChangeEventArgs.Change.Coordinates);
    }

    private void SpawnCollidersXY(Vector2Int coordinates) {
      var cellIndex = _mapIndexService.CellToIndex(coordinates);
      var columnCount = _columnTerrainMap.ColumnCount[cellIndex];
      for (var i = 0; i < columnCount; i++) {
        var index3D = cellIndex + i * _mapIndexService.VerticalStride;
        SpawnCollider(index3D, _columnTerrainMap.GetColumn(index3D));
      }
    }

    private void SpawnCollider(int index, TerrainColumn terrainColumn) {
      var coordinates = _mapIndexService.Index3DToCoordinates(index);
      var position =
          CoordinateSystem.GridToWorldCentered(
              new Vector3(coordinates.x, coordinates.y, terrainColumn.Floor));
      var size = new Vector3(1, terrainColumn.Ceiling - terrainColumn.Floor, 1);
      var collider = _rootObject.AddComponent<BoxCollider>();
      collider.center = position + new Vector3(0, size.y / 2f, 0);
      collider.size = size;
      _boxColliders.Add(index, collider);
    }

    private void RemoveCollidersXY(Vector2Int coordinates) {
      var cellIndex = _mapIndexService.CellToIndex(coordinates);
      var columnCount = _columnTerrainMap.ColumnCount[cellIndex];
      for (var i = 0; i < columnCount; i++) {
        var index3D = cellIndex + i * _mapIndexService.VerticalStride;
        Object.Destroy(_boxColliders[index3D]);
        _boxColliders.Remove(index3D);
      }
    }

  }
}