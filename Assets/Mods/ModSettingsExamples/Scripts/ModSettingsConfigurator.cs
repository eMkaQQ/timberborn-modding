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
      containerDefinition.Bind<ValueChangeListenerExample>().AsSingleton();
    }

  }
}