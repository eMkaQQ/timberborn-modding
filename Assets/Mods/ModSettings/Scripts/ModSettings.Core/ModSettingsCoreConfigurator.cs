using Bindito.Core;

namespace ModSettings.Core {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  public class ModSettingsCoreConfigurator : Configurator {

    protected override void Configure() {
      Bind<ModSettingsOwnerRegistry>().AsSingleton();
    }

  }
}