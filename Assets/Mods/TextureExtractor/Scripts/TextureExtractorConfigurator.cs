using Bindito.Core;

namespace Mods.TextureExtractor.Scripts {
  [Context("MainMenu")]
  internal class TextureExtractorConfigurator : Configurator {

    protected override void Configure() {
      Bind<NormalMapExtractor>().AsSingleton();
    }

  }
}