using Timberborn.BlueprintSystem;

namespace Riverborne.Core {
  internal record RaftDockSpec : ComponentSpec {

    [Serialize]
    public string DropPointName { get; init; }

    [Serialize]
    public string StaticRaftName { get; init; }

  }
}