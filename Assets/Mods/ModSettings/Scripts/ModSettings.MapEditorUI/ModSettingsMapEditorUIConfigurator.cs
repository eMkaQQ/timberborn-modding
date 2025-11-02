using Bindito.Core;
using ModSettings.Core;

namespace ModSettings.MapEditorUI {
  [Context("MapEditor")]
  internal class ModSettingsMapEditorUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<MapEditorModManagerOpener>().AsSingleton();
      Bind<IModSettingsContextProvider>()
          .To<MapEditorModSettingsContext>()
          .AsSingleton();
    }

  }
}