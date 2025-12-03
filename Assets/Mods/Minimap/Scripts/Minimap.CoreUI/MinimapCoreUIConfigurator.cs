using Bindito.Core;
using ModSettings.CoreUI;

namespace Minimap.CoreUI {
  [Context("Game")]
  [Context("MapEditor")]
  public class MinimapCoreUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<MinimapElement>().AsSingleton();
      Bind<MinimapElementRotator>().AsSingleton();
      Bind<MinimapRotationSettingOwner>().AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<MinimapRotationSettingElementFactory>()
          .AsSingleton();
      Bind<MinimapRotationSettingElementFactory>().AsSingleton();
      Bind<TutorialPanelsAdjuster>().AsSingleton();
    }

  }
}