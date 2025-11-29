using Bindito.Core;
using Timberborn.TemplateInstantiation;
using Timberborn.WorkSystem;

namespace SecondShift.Core {
  [Context("Game")]
  internal class SecondShiftCoreConfigurator : Configurator {

    protected override void Configure() {
      Bind<TwoShiftsWorkingHours>().AsTransient();
      Bind<TwoShiftsWorkplace>().AsTransient();
      Bind<WorkerPositionFixer>().AsTransient();

      MultiBind<TemplateModule>()
          .ToProvider(ProvideTemplateModule)
          .AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<WorkerWorkingHours, TwoShiftsWorkingHours>();
      builder.AddDecorator<Workplace, TwoShiftsWorkplace>();
      builder.AddDecorator<Worker, WorkerPositionFixer>();
      return builder.Build();
    }

  }
}