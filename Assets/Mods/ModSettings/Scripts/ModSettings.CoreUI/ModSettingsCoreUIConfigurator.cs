using Bindito.Core;

namespace ModSettings.CoreUI {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  internal class ModSettingsCoreUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ModSettingsBox>().AsSingleton();
      containerDefinition.Bind<ModItemSettingsInitializer>().AsSingleton();
    }

  }
}