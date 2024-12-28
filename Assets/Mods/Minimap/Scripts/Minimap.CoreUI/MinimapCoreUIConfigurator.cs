using Bindito.Core;
using ModSettings.CoreUI;

namespace Minimap.CoreUI {
  [Context("Game")]
  [Context("MapEditor")]
  public class MinimapCoreUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<MinimapElement>().AsSingleton();
      containerDefinition.Bind<MinimapElementRotator>().AsSingleton();
      containerDefinition.Bind<MinimapRotationSettingOwner>().AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<MinimapRotationSettingElementFactory>()
          .AsSingleton();
      containerDefinition.Bind<MinimapRotationSettingElementFactory>().AsSingleton();
    }

  }
}