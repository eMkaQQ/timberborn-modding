using Timberborn.BaseComponentSystem;
using Timberborn.BlueprintSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using UnityEngine;

namespace Riverborne.Core {
  public class RaftSpawner : BaseComponent,
                             IAwakableComponent {

    private readonly ISpecService _specService;
    private readonly EntityService _entityService;
    private RaftDock _raftDock;
    private Transform _dropPoint;
    private Blueprint _blueprint;

    public RaftSpawner(ISpecService specService,
                       EntityService entityService) {
      _specService = specService;
      _entityService = entityService;
    }

    public Vector3 DropPoint => _dropPoint.position;

    public void Awake() {
      _raftDock = GetComponent<RaftDock>();
      var spec = GetComponent<RaftDockSpec>();
      _dropPoint = GameObject.FindChildTransform(spec.DropPointName);
      _blueprint = _specService.GetBlueprint("Riverborne/Raft/Raft.blueprint");
    }

    public Raft Spawn(RaftDispatch raftDispatch) {
      var raft = _entityService.Instantiate(_blueprint);
      raft.Transform.SetPositionAndRotation(_dropPoint.position, _dropPoint.rotation);
      var raftComponent = raft.GetComponent<Raft>();
      raftComponent.Initialize(raftDispatch.Name, _raftDock, raftDispatch.Cargo);
      return raftComponent;
    }

  }
}