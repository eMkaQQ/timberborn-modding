using Bindito.Core;
using Minimap.Core;
using Timberborn.Buildings;
using Timberborn.Fields;
using Timberborn.Forestry;
using Timberborn.TemplateInstantiation;

namespace Minimap.Renderers {
  [Context("Game")]
  [Context("MapEditor")]
  internal class MinimapRenderersConfigurator : Configurator {

    protected override void Configure() {
      Bind<TreeMinimapRenderer>().AsTransient();
      Bind<PlantMinimapRenderer>().AsTransient();
      Bind<BuildingMinimapRenderer>().AsTransient();
      
      Bind<IMinimapBlockObjectRenderer>()
          .To<BlockObjectMinimapRenderer>()
          .AsSingleton();
      MultiBind<TemplateModule>()
          .ToProvider(ProvideTemplateModule)
          .AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<TreeComponentSpec, TreeMinimapRenderer>();
      builder.AddDecorator<BushSpec, PlantMinimapRenderer>();
      builder.AddDecorator<Crop, PlantMinimapRenderer>();
      builder.AddDecorator<BuildingSpec, BuildingMinimapRenderer>();
      return builder.Build();
    }

  }
}