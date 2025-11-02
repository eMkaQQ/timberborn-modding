using Bindito.Core;

namespace ModSettings.CoreUI {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  internal class ModSettingsCoreUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<ModSettingsBox>().AsSingleton();
      Bind<ModItemSettingsInitializer>().AsSingleton();
    }

  }
}