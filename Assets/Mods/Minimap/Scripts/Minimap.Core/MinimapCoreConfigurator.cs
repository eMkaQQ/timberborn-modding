using Bindito.Core;

namespace Minimap.Core {
  [Context("Game")]
  [Context("MapEditor")]
  internal class MinimapCoreConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<MinimapTexture>().AsSingleton();
      containerDefinition.Bind<MinimapRenderer>().AsSingleton();
      containerDefinition.Bind<TopBlockObjectsRegistry>().AsSingleton();
    }

  }
}