using Bindito.Core;

namespace ModSettings.ColorPicker {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  internal class ColorPickerConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<ColorPickerShower>().AsSingleton();
      containerDefinition.Bind<ColorPickerFactory>().AsSingleton();
    }

  }
}