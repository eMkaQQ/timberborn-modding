using Bindito.Core;

namespace ModSettings.MainMenuUI {
  [Context("MainMenu")]
  internal class ModSettingsCoreUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ModItemSettingsInitializer>().AsSingleton();
    }

  }
}