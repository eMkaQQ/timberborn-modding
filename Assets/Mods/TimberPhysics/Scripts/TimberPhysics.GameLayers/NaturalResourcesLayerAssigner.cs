using JetBrains.Annotations;
using TimberPhysics.Layers;

namespace TimberPhysics.GameLayers {
  public class NaturalResourcesLayerAssigner : IPhysicalObjectLayerProvider {

    [UsedImplicitly]
    public static readonly string LayerNameValue = "NaturalResources";
    public string LayerName => LayerNameValue;

  }
}