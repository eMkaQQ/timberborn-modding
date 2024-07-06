using Bindito.Core;
using ModSettingsUI;

namespace ModSettingsFactoriesUI {
  [Context("MainMenu")]
  public class ModSettingsFactoriesUIConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
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
    }

  }
}