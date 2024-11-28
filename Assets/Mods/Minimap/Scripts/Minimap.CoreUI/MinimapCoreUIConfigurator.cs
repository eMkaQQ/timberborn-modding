using Bindito.Core;

namespace Minimap.CoreUI {
  [Context("Game")]
  [Context("MapEditor")]
  public class MinimapCoreUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<MinimapElement>().AsSingleton();
    }

  }
}