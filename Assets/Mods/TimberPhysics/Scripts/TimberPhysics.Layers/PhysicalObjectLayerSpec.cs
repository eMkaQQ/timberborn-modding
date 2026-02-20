using Timberborn.BlueprintSystem;

namespace TimberPhysics.Layers {
  internal record PhysicalObjectLayerSpec : ComponentSpec,
                                            IPhysicalObjectLayerProvider {

    [Serialize]
    public string LayerName { get; init; }

  }
}