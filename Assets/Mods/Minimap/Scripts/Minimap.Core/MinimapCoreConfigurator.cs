using Bindito.Core;

namespace Minimap.Core {
  [Context("Game")]
  [Context("MapEditor")]
  internal class MinimapCoreConfigurator : Configurator {

    protected override void Configure() {
      Bind<MinimapTexture>().AsSingleton();
      Bind<MinimapRenderer>().AsSingleton();
      Bind<TopBlockObjectsRegistry>().AsSingleton();
    }

  }
}