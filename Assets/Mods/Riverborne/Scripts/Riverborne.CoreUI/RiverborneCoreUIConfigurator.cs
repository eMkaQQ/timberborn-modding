using Bindito.Core;
using Riverborne.Core;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateInstantiation;

namespace Riverborne.CoreUI {
  [Context("Game")]
  internal class RiverborneCoreUIConfigurator : Configurator {

    protected override void Configure() {
      Bind<RaftDescriber>().AsTransient();

      Bind<RaftFragment>().AsSingleton();
      Bind<RaftDockFragment>().AsSingleton();
      Bind<EditDispatchPanel>().AsSingleton();
      Bind<EditDispatchPanelGoodFactory>().AsSingleton();
      Bind<RaftDispatchItemFactory>().AsSingleton();
      Bind<RaftDockInventoryFragment>().AsSingleton();
      Bind<RaftDockInventoryRowUpdater>().AsSingleton();
      Bind<DeleteRaftFragment>().AsSingleton();

      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
      MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<RaftSpec, RaftDescriber>();
      return builder.Build();
    }

    private class EntityPanelModuleProvider : IProvider<EntityPanelModule> {

      private readonly RaftFragment _raftFragment;
      private readonly RaftDockFragment _raftDockFragment;
      private readonly RaftDockInventoryFragment _raftDockInventoryFragment;
      private readonly DeleteRaftFragment _deleteRaftFragment;

      public EntityPanelModuleProvider(RaftFragment raftFragment,
                                       RaftDockFragment raftDockFragment,
                                       RaftDockInventoryFragment raftDockInventoryFragment,
                                       DeleteRaftFragment deleteRaftFragment) {
        _raftFragment = raftFragment;
        _raftDockFragment = raftDockFragment;
        _raftDockInventoryFragment = raftDockInventoryFragment;
        _deleteRaftFragment = deleteRaftFragment;
      }

      public EntityPanelModule Get() {
        var builder = new EntityPanelModule.Builder();
        builder.AddLeftHeaderFragment(_deleteRaftFragment, 0);
        builder.AddBottomFragment(_raftFragment);
        builder.AddSideFragment(_raftDockFragment);
        builder.AddBottomFragment(_raftDockInventoryFragment);
        return builder.Build();
      }

    }

  }
}