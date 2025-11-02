using Bindito.Core;
using ModSettings.Core;

namespace ModSettings.MainMenuUI {
  [Context("MainMenu")]
  internal class ModSettingsCoreUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<IModSettingsContextProvider>()
          .To<MainMenuModSettingsContext>()
          .AsSingleton();
    }

  }
}