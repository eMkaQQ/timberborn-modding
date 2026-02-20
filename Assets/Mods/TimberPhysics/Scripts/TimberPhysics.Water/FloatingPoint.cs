using Timberborn.BlueprintSystem;
using UnityEngine;

namespace TimberPhysics.Water {
  internal record FloatingPoint {

    [Serialize]
    public Vector2 Position { get; init; }

    [Serialize]
    public float Weight { get; init; }

    public bool IsCenterPoint => Position.sqrMagnitude < float.Epsilon;

  }
}