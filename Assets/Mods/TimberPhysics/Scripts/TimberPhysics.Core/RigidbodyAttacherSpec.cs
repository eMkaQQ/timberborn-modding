using Timberborn.BlueprintSystem;
using UnityEngine;

namespace TimberPhysics.Core {
  internal record RigidbodyAttacherSpec : ComponentSpec {

    [Serialize]
    public AssetRef<Rigidbody> Rigidbody { get; init; }

  }
}