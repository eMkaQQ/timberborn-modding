using Bindito.Core;
using ModSettings.Core;

namespace ModSettings.GameUI {
  [Context("Game")]
  internal class ModSettingsGameUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<GameModManagerOpener>().AsSingleton();
      Bind<IModSettingsContextProvider>()
          .To<GameModSettingsContext>()
          .AsSingleton();
    }

  }
}