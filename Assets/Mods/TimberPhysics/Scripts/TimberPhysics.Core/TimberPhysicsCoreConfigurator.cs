using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.StatusSystem;
using Timberborn.TemplateInstantiation;

namespace TimberPhysics.Core {
  [Context("Game")]
  internal class TimberPhysicsCoreConfigurator : Configurator {

    protected override void Configure() {
      Bind<RigidbodyAttacher>().AsTransient();
      Bind<PersistentRigidbody>().AsTransient();
      Bind<BoxColliderFixer>().AsTransient();
      Bind<StatusIconFixer>().AsTransient();
      Bind<PhysicalObjectRegistrar>().AsTransient();

      Bind<PhysicsSimulator>().AsSingleton();
      Bind<PhysicalObjectRegistry>().AsSingleton();

      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<RigidbodyAttacherSpec, RigidbodyAttacher>();
      builder.AddDecorator<RigidbodyAttacher, PersistentRigidbody>();
      builder.AddDecorator<BlockObject, BoxColliderFixer>();
      builder.AddDecorator<StatusIconCycler, StatusIconFixer>();
      builder.AddDecorator<IPhysicalObject, PhysicalObjectRegistrar>();
      return builder.Build();
    }

  }
}