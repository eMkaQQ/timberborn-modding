using Bindito.Core;
using Timberborn.BottomBarSystem;
using Timberborn.Emptying;
using Timberborn.EntityPanelSystem;
using Timberborn.Hauling;
using Timberborn.InventoryNeedSystem;
using Timberborn.LaborSystem;
using Timberborn.Rendering;
using Timberborn.SelectionSystem;
using Timberborn.TemplateInstantiation;
using Timberborn.Workshops;
using Timberborn.WorkSystem;

namespace Riverborne.Core {
  [Context("Game")]
  internal class RiverborneCoreConfigurator : Configurator {

    protected override void Configure() {
      Bind<Raft>().AsTransient();
      Bind<RaftModel>().AsTransient();
      Bind<RaftDock>().AsTransient();
      Bind<RaftDockInventory>().AsTransient();
      Bind<RaftDockHaulBehaviorProvider>().AsTransient();
      Bind<RaftSpawner>().AsTransient();
      Bind<RaftDispatcher>().AsTransient();
      Bind<RaftPicker>().AsTransient();
      Bind<RaftDischarger>().AsTransient();
      Bind<RaftRamp>().AsTransient();
      Bind<RaftRampAnimator>().AsTransient();

      Bind<RaftDispatchSerializer>().AsSingleton();
      Bind<RaftDockInventoryInitializer>().AsSingleton();
      Bind<RaftCargoRowFactory>().AsSingleton();
      Bind<RaftPickingService>().AsSingleton();

      MultiBind<TemplateModule>().ToProvider<TemplateModuleProvider>().AsSingleton();
    }

    private class TemplateModuleProvider : IProvider<TemplateModule> {

      private readonly RaftDockInventoryInitializer _raftDockInventoryInitializer;

      public TemplateModuleProvider(RaftDockInventoryInitializer raftDockInventoryInitializer) {
        _raftDockInventoryInitializer = raftDockInventoryInitializer;
      }

      public TemplateModule Get() {
        var builder = new TemplateModule.Builder();
        builder.AddDedicatedDecorator(_raftDockInventoryInitializer);
        builder.AddDecorator<RaftSpec, SelectableObject>();
        builder.AddDecorator<RaftSpec, LabeledEntityBadge>();
        builder.AddDecorator<RaftSpec, Raft>();
        builder.AddDecorator<Raft, RaftModel>();
        builder.AddDecorator<Raft, RaftDischarger>();
        builder.AddDecorator<RaftModel, EntityMaterials>();
        builder.AddDecorator<RaftDockSpec, RaftDock>();
        builder.AddDecorator<RaftDock, InventoryNeedBehavior>();
        builder.AddDecorator<RaftDock, RaftDockInventory>();
        builder.AddDecorator<RaftDock, AutoEmptiable>();
        builder.AddDecorator<RaftDock, Emptiable>();
        builder.AddDecorator<RaftDock, FillInputHaulBehaviorProvider>();
        builder.AddDecorator<RaftDock, RaftDockHaulBehaviorProvider>();
        builder.AddDecorator<RaftDock, RaftSpawner>();
        builder.AddDecorator<RaftDock, RaftDispatcher>();
        builder.AddDecorator<RaftDock, WorkplaceWithBackpacks>();
        builder.AddDecorator<RaftPickerSpec, RaftPicker>();
        builder.AddDecorator<RaftRampSpec, RaftRamp>();
        builder.AddDecorator<RaftRampSpec, RaftRampAnimator>();
        InitializeBehaviors(builder);
        return builder.Build();
      }

      private static void InitializeBehaviors(TemplateModule.Builder builder) {
        builder.AddDecorator<RaftDock, FillInputWorkplaceBehavior>();
        builder.AddDecorator<RaftDock, EmptyOutputWorkplaceBehavior>();
        builder.AddDecorator<RaftDock, RemoveUnwantedStockWorkplaceBehavior>();
        builder.AddDecorator<RaftDock, EmptyInventoriesWorkplaceBehavior>();
        builder.AddDecorator<RaftDock, LaborWorkplaceBehavior>();
        builder.AddDecorator<RaftDock, WaitInsideIdlyWorkplaceBehavior>();
      }

    }

  }
}