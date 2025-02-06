using Bindito.Core;
using Timberborn.MainMenuModdingUI;
using Timberborn.ModdingUI;

namespace ModSettings.ModManager {
  [Context("Game")]
  [Context("MapEditor")]
  internal class NotMainMenuModdingUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<ModManagerBox>().AsSingleton();
      Bind<ModUploaderBox>().AsSingleton();
      Bind<IModManagerTooltipRegistrar>().To<ModManagerBoxTooltipRegistrar>().AsSingleton();
      Bind<IModItemFactory>().To<MainMenuModItemFactory>().AsSingleton();
    }

  }
}