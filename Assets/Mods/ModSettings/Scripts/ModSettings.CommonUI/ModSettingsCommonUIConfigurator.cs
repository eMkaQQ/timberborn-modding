using Bindito.Core;
using ModSettings.CoreUI;

namespace ModSettings.CommonUI {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  internal class ModSettingsCommonUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<ModSettingDescriptorInitializer>().AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<BoolModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<FloatModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<StringModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<IntModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<DropdownModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<SliderIntModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<LongStringModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<ColorModSettingElementFactory>()
          .AsSingleton();
      MultiBind<IModSettingElementFactory>()
          .To<ReadonlyTextModSettingElementFactory>()
          .AsSingleton();
    }

  }
}