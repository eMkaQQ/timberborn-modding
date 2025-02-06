using Bindito.Core;
using Timberborn.ModdingUI;

namespace ModSettings.ModManager {
  [Context("Game")]
  [Context("MapEditor")]
  internal class NotModdingUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<ModListView>().AsSingleton();
    }

  }
}