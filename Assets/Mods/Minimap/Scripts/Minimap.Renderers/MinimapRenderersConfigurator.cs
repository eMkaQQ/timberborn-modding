using Bindito.Core;
using Minimap.Core;
using Timberborn.Buildings;
using Timberborn.Fields;
using Timberborn.Forestry;
using Timberborn.TemplateSystem;

namespace Minimap.Renderers {
  [Context("Game")]
  [Context("MapEditor")]
  internal class MinimapRenderersConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<IMinimapBlockObjectRenderer>()
          .To<BlockObjectMinimapRenderer>()
          .AsSingleton();
      containerDefinition.MultiBind<TemplateModule>()
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