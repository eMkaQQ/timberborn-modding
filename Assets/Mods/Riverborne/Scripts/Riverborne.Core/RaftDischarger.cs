using Timberborn.BaseComponentSystem;
using Timberborn.Navigation;
using Timberborn.TickSystem;

namespace Riverborne.Core {
  internal class RaftDischarger : TickableComponent,
                                  IAwakableComponent,
                                  IUpdatableComponent {

    private readonly RaftPickingService _raftPickingService;
    private Raft _raft;
    private RaftPicker _lastRaftPicker;

    public RaftDischarger(RaftPickingService raftPickingService) {
      _raftPickingService = raftPickingService;
    }

    public void Awake() {
      _raft = GetComponent<Raft>();
    }

    public void Update() {
      if (_lastRaftPicker == null) {
        var coordinates = NavigationCoordinateSystem.WorldToGridInt(Transform.position);
        _lastRaftPicker = _raftPickingService.GetRaftPicker(coordinates);
      }
    }

    public override void Tick() {
      if (_lastRaftPicker != null) {
        _lastRaftPicker.TryPickRaft(_raft);
        _lastRaftPicker = null;
      }
    }

  }
}