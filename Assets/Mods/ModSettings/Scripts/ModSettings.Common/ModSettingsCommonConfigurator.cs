using Bindito.Core;

namespace ModSettings.Common {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  public class ModSettingsCommonConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<DefaultModFileStoredSettings>().AsSingleton();
    }

  }
}