using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.Characters;
using Timberborn.NaturalResources;
using Timberborn.TemplateInstantiation;

namespace TimberPhysics.GameLayers {
  [Context("Game")]
  internal class TimberPhysicsGameLayersConfigurator : Configurator {

    protected override void Configure() {
      Bind<CharactersLayerAssigner>().AsTransient();
      Bind<BlockObjectsLayerAssigner>().AsTransient();
      Bind<NaturalResourcesLayerAssigner>().AsTransient();

      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<Character, CharactersLayerAssigner>();
      builder.AddDecorator<NaturalResource, NaturalResourcesLayerAssigner>();
      builder.AddDecorator<BlockObject, BlockObjectsLayerAssigner>();
      return builder.Build();
    }

  }
}