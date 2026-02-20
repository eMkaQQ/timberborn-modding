using System.Collections.Immutable;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.GoodStackSystem;
using Timberborn.InventorySystem;
using Timberborn.Persistence;
using Timberborn.WorldPersistence;

namespace Riverborne.Core {
  public class Raft : BaseComponent,
                      IInitializableEntity,
                      IGoodStackInventory,
                      IPersistentEntity {

    public static readonly string MaterialGood = "Log";
    public static readonly int MaterialAmount = 4;
    private static readonly ComponentKey RaftKey = new("Riverborne.Raft");
    private static readonly PropertyKey<string> NameKey = new("Name");
    private static readonly PropertyKey<RaftDock> OriginDockKey = new("OriginDock");
    public Inventory Inventory { get; private set; }
    public RaftDock OriginDock { get; private set; }
    private readonly ReferenceSerializer _referenceSerializer;
    private string _name;
    private ImmutableArray<GoodAmount> _cargo;

    public Raft(ReferenceSerializer referenceSerializer) {
      _referenceSerializer = referenceSerializer;
    }

    public void Initialize(string name, RaftDock raftDock, ImmutableArray<GoodAmount> cargo) {
      _name = name;
      OriginDock = raftDock;
      _cargo = cargo;
    }

    public void InitializeEntity() {
      if (!_cargo.IsDefault) {
        foreach (var goodAmount in _cargo) {
          Inventory.Give(goodAmount);
        }
      }
    }

    public void InitializeInventory(Inventory inventory) {
      Asserts.FieldIsNull(this, Inventory, nameof(Inventory));
      Inventory = inventory;
      Inventory.Enable();
    }

    public void Save(IEntitySaver entitySaver) {
      var raft = entitySaver.GetComponent(RaftKey);
      raft.Set(NameKey, _name);
      if (OriginDock != null) {
        raft.Set(OriginDockKey, OriginDock, _referenceSerializer.Of<RaftDock>());
      }
    }

    public void Load(IEntityLoader entityLoader) {
      var raft = entityLoader.GetComponent(RaftKey);
      _name = raft.Get(NameKey);
      if (raft.Has(OriginDockKey)
          && raft.GetObsoletable(OriginDockKey, _referenceSerializer.Of<RaftDock>(),
                                 out var dock)) {
        OriginDock = dock;
      }
    }

  }
}