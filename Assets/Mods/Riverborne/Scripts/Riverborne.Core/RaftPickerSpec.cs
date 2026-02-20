using System.Collections.Immutable;
using Timberborn.BlueprintSystem;
using UnityEngine;

namespace Riverborne.Core {
  internal record RaftPickerSpec : ComponentSpec {

    [Serialize]
    public ImmutableArray<Vector3Int> PickerCoordinates { get; init; }

  }
}