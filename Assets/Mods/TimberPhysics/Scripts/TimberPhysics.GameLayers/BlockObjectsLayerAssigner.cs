using JetBrains.Annotations;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using TimberPhysics.Layers;

namespace TimberPhysics.GameLayers {
  public class BlockObjectsLayerAssigner : BaseComponent,
                                           IAwakableComponent,
                                           IFinishedStateListener {

    [UsedImplicitly]
    public static readonly string FinishedLayerNameValue = "FinishedBlockObjects";
    [UsedImplicitly]
    public static readonly string UnfinishedLayerNameValue = "UnfinishedBlockObjects";
    private readonly PhysicsLayerRegistry _physicsLayerRegistry;

    public BlockObjectsLayerAssigner(PhysicsLayerRegistry physicsLayerRegistry) {
      _physicsLayerRegistry = physicsLayerRegistry;
    }

    public void Awake() {
      SetLayerIfNotOverridden(UnfinishedLayerNameValue);
    }

    public void OnEnterFinishedState() {
      SetLayerIfNotOverridden(FinishedLayerNameValue);
    }

    public void OnExitFinishedState() {
      SetLayerIfNotOverridden(UnfinishedLayerNameValue);
    }

    private void SetLayerIfNotOverridden(string layerName) {
      if (GameObject.layer <= PhysicsLayerRegistry.DefaultLayersIndex
          || _physicsLayerRegistry.TryGetLayerIndex(FinishedLayerNameValue, out var index)
          && index == GameObject.layer
          || _physicsLayerRegistry.TryGetLayerIndex(UnfinishedLayerNameValue, out index)
          && index == GameObject.layer) {
        _physicsLayerRegistry.AssignGameObjectToLayer(GameObject, layerName);
      }
    }

  }
}