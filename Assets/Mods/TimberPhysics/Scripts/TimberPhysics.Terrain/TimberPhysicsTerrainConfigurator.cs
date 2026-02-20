using Bindito.Core;
using Timberborn.Debugging;

namespace TimberPhysics.Terrain {
  [Context("Game")]
  internal class TimberPhysicsTerrainConfigurator : Configurator {

    protected override void Configure() {
      Bind<TerrainColliderService>().AsSingleton();
      MultiBind<IDevModule>().To<TerrainColliderDisabler>().AsSingleton();
    }

  }
}