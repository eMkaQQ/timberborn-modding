using Bindito.Core;

namespace Minimap.Settings {
  [Context("Game")]
  [Context("MapEditor")]
  [Context("MainMenu")]
  internal class MinimapSettingsConfigurator : Configurator {

    protected override void Configure() {
      Bind<MinimapColorSettings>().AsSingleton();
      Bind<MinimapSettings>().AsSingleton();
    }

  }
}