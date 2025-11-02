using Bindito.Core;

namespace ModSettings.ColorPicker {
  [Context("MainMenu")]
  [Context("Game")]
  [Context("MapEditor")]
  internal class ColorPickerConfigurator : Configurator {

    protected override void Configure() {
      Bind<ColorPickerShower>().AsSingleton();
      Bind<ColorPickerFactory>().AsSingleton();
    }

  }
}