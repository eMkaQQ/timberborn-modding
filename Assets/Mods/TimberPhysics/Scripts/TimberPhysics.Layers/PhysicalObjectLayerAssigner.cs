using Timberborn.BaseComponentSystem;

namespace TimberPhysics.Layers {
  public class PhysicalObjectLayerAssigner : BaseComponent,
                                             IAwakableComponent {

    private readonly PhysicsLayerRegistry _physicsLayerRegistry;

    public PhysicalObjectLayerAssigner(PhysicsLayerRegistry physicsLayerRegistry) {
      _physicsLayerRegistry = physicsLayerRegistry;
    }

    public void Awake() {
      var layerProvider = GetComponent<IPhysicalObjectLayerProvider>();
      _physicsLayerRegistry.AssignGameObjectToLayer(GameObject, layerProvider.LayerName);
    }

  }
}