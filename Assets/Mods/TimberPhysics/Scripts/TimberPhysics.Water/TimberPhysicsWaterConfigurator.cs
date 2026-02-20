using Bindito.Core;
using Timberborn.TemplateInstantiation;

namespace TimberPhysics.Water {
  [Context("Game")]
  internal class TimberPhysicsWaterConfigurator : Configurator {

    protected override void Configure() {
      Bind<FloatingObject>().AsTransient();

      Bind<FloatingObjectLayerRegistrar>().AsSingleton();

      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<FloatingObjectSpec, FloatingObject>();
      return builder.Build();
    }

  }
}