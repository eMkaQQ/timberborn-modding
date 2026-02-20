using Timberborn.Common;
using Timberborn.Debugging;

namespace TimberPhysics.Terrain {
  internal class TerrainColliderDisabler : IDevModule {

    private readonly TerrainColliderService _terrainColliderService;

    public TerrainColliderDisabler(TerrainColliderService terrainColliderService) {
      _terrainColliderService = terrainColliderService;
    }

    public DevModuleDefinition GetDefinition() {
      return new(Enumerables.One(new DevMethod("Toggle Terrain Colliders", null,
                                               ToggleTerrainCollider)));
    }

    private void ToggleTerrainCollider() {
      _terrainColliderService.ToggleColliders();
    }

  }
}