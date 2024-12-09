using Bindito.Core;
using ModSettings.Core;

namespace ModSettings.GameUI {
  [Context("Game")]
  internal class ModSettingsGameUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GameModManagerOpener>().AsSingleton();
      containerDefinition.Bind<IModSettingsContextProvider>()
          .To<GameModSettingsContext>()
          .AsSingleton();
    }

  }
}