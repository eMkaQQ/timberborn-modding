using JetBrains.Annotations;
using TimberPhysics.Layers;

namespace TimberPhysics.GameLayers {
  public class CharactersLayerAssigner : IPhysicalObjectLayerProvider {

    [UsedImplicitly]
    public static readonly string LayerNameValue = "Characters";
    public string LayerName => LayerNameValue;

  }
}