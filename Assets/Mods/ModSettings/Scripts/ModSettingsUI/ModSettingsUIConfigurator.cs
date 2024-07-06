using Bindito.Core;

namespace ModSettingsUI {
  [Context("MainMenu")]
  public class ModSettingsUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ModSettingsBox>().AsSingleton();
      containerDefinition.Bind<ModItemSettingsInitializer>().AsSingleton();
    }

  }
}