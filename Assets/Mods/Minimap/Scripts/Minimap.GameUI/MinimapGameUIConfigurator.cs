using Bindito.Core;

namespace Minimap.GameUI {
  [Context("Game")]
  public class MinimapGameUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<TutorialPanelsAdjuster>().AsSingleton();
    }

  }
}