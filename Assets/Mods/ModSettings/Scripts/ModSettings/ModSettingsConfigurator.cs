using Bindito.Core;

namespace ModSettings {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  public class ModSettingsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ModSettingsOwnerRegistry>().AsSingleton();
    }

  }
}