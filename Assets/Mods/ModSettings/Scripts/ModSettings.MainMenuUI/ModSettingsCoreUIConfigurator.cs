using Bindito.Core;
using ModSettings.Core;

namespace ModSettings.MainMenuUI {
  [Context("MainMenu")]
  internal class ModSettingsCoreUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<IModSettingsContextProvider>()
          .To<MainMenuModSettingsContext>()
          .AsSingleton();
    }

  }
}