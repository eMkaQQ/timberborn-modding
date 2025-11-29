using Bindito.Core;
using Timberborn.EntityPanelSystem;

namespace SecondShift.CoreUI {
  [Context("Game")]
  internal class SecondShiftCoreUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<TwoShiftsWorkplaceFragment>().AsSingleton();
      Bind<TwoShiftsWorkersToggle>().AsTransient();

      MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }

    private class EntityPanelModuleProvider : IProvider<EntityPanelModule> {

      private readonly TwoShiftsWorkplaceFragment _twoShiftsWorkplaceFragment;

      public EntityPanelModuleProvider(TwoShiftsWorkplaceFragment twoShiftsWorkplaceFragment) {
        _twoShiftsWorkplaceFragment = twoShiftsWorkplaceFragment;
      }

      public EntityPanelModule Get() {
        var builder = new EntityPanelModule.Builder();
        builder.AddTopFragment(_twoShiftsWorkplaceFragment);
        return builder.Build();
      }

    }

  }
}