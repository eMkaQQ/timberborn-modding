using System.Collections.Immutable;
using Timberborn.BlueprintSystem;

namespace Riverborne.Core {
  internal record RaftRampSpec : ComponentSpec {

    [Serialize]
    public int MaxRampSpeed { get; init; }

    [Serialize]
    public int Acceleration { get; init; }

    [Serialize]
    public string LaunchingObjectName { get; init; }
    
    [Serialize]
    public float AnimationSpeed { get; init; }

  }
}