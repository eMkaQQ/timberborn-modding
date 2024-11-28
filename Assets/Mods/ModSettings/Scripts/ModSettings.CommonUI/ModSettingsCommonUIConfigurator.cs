using Bindito.Core;
using ModSettings.CoreUI;

namespace ModSettings.CommonUI {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  internal class ModSettingsCommonUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ModSettingDescriptorInitializer>().AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<BoolModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<FloatModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<StringModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<IntModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<DropdownModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<SliderIntModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<LongStringModSettingElementFactory>()
          .AsSingleton();
      containerDefinition.MultiBind<IModSettingElementFactory>()
          .To<ColorModSettingElementFactory>()
          .AsSingleton();
    }

  }
}