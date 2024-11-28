using Bindito.Core;

namespace Minimap.Settings {
  [Context("Game")]
  [Context("MapEditor")]
  [Context("MainMenu")]
  internal class MinimapSettingsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<MinimapColorSettings>().AsSingleton();
      containerDefinition.Bind<MinimapSettings>().AsSingleton();
    }

  }
}