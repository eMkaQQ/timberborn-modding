using Timberborn.BlueprintSystem;

namespace Riverborne.Core {
  internal record RaftModelHiderSpec : ComponentSpec {

    [Serialize]
    public string ModelName { get; init; }

  }
}