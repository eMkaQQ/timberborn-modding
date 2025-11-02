using Bindito.Core;

namespace ModSettings.Common {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  public class ModSettingsCommonConfigurator : Configurator {

    protected override void Configure() {
      Bind<DefaultModFileStoredSettings>().AsSingleton();
    }

  }
}