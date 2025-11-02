using Bindito.Core;
using ModSettings.CoreUI;

namespace ModSettingsExamples {
  [Context("MainMenu")]
  [Context("Game")]
  internal class ModSettingsConfigurator : Configurator {

    protected override void Configure() {
      Bind<AdvancedSettingsExample>().AsSingleton();
      Bind<SimpleSettingsExample>().AsSingleton();
      Bind<SettingsLogger>().AsSingleton();
      Bind<FileStoredSettingsExample>().AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<DummyButtonElementFactory>()
          .AsSingleton();
    }

  }

  [Context("MainMenu")]
  internal class MainMenuConfigurator : Configurator {

    protected override void Configure() {
      Bind<ValueChangeListenerExample>().AsSingleton();
    }

  }
}