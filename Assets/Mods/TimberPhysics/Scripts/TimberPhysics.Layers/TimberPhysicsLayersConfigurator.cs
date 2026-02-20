using Bindito.Core;
using Timberborn.TemplateInstantiation;

namespace TimberPhysics.Layers {
  [Context("Game")]
  internal class TimberPhysicsLayersConfigurator : Configurator {

    protected override void Configure() {
      Bind<PhysicalObjectLayerAssigner>().AsTransient();

      Bind<PhysicsLayerRegistry>().AsSingleton();

      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<IPhysicalObjectLayerProvider, PhysicalObjectLayerAssigner>();
      return builder.Build();
    }

  }
}