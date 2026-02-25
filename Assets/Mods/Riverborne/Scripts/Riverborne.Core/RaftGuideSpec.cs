using Timberborn.BlueprintSystem;

namespace Riverborne.Core {
  internal record RaftGuideSpec : ComponentSpec {

    [Serialize]
    public bool FlippedOrientationSupports { get; init; }

  }
}