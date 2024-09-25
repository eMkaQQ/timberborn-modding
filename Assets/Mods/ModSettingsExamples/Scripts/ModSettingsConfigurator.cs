using Bindito.Core;

namespace ModSettingsExamples {
  [Context("MainMenu")]
  [Context("Game")]
  internal class ModSettingsConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<AdvancedSettingsExample>().AsSingleton();
      containerDefinition.Bind<SimpleSettingsExample>().AsSingleton();
      containerDefinition.Bind<SettingsLogger>().AsSingleton();
      containerDefinition.Bind<FileStoredSettingsExample>().AsSingleton();
    }

  }
  
  [Context("MainMenu")]
  internal class MainMenuConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ValueChangeListenerExample>().AsSingleton();
    }

  }
}