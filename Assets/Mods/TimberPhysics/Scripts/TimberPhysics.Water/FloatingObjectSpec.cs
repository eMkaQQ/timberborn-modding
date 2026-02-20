using System.Collections.Immutable;
using Timberborn.BlueprintSystem;
using UnityEngine;

namespace TimberPhysics.Water {
  internal record FloatingObjectSpec : ComponentSpec {

    [Serialize]
    public ImmutableArray<FloatingPoint> FloatingPoints { get; init; }

    [Serialize]
    public float Immersion { get; init; }

    [Serialize]
    public float PointStiffness { get; init; }

    [Serialize]
    public float PointDamping { get; init; }

    [Serialize]
    public float WaterLinearDamping { get; init; }

    [Serialize]
    public float WaterAngularDamping { get; init; }

    [Serialize]
    public float CurrentForceMultiplier { get; init; }

    [Serialize]
    public AssetRef<PhysicsMaterial> PhysicsMaterial { get; init; }

  }
}