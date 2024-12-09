using Bindito.Core;
using ModSettings.Core;

namespace ModSettings.MapEditorUI {
  [Context("MapEditor")]
  internal class ModSettingsMapEditorUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<MapEditorModManagerOpener>().AsSingleton();
      containerDefinition.Bind<IModSettingsContextProvider>()
          .To<MapEditorModSettingsContext>()
          .AsSingleton();
    }

  }
}