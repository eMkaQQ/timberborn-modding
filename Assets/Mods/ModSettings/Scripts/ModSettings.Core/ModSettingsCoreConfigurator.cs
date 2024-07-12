using Bindito.Core;

namespace ModSettings.Core {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  public class ModSettingsCoreConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ModSettingsOwnerRegistry>().AsSingleton();
    }

  }
}