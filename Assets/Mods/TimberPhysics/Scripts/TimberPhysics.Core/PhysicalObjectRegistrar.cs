using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;

namespace TimberPhysics.Core {
  internal class PhysicalObjectRegistrar : BaseComponent,
                                           IAwakableComponent,
                                           IInitializableEntity,
                                           IFinishedStateListener,
                                           IDeletableEntity {

    private readonly PhysicalObjectRegistry _physicalObjectRegistry;
    private List<IPhysicalObject> _physicalObjects;
    private bool _isBlockObject;

    public PhysicalObjectRegistrar(PhysicalObjectRegistry physicalObjectRegistry) {
      _physicalObjectRegistry = physicalObjectRegistry;
    }

    public void Awake() {
      _isBlockObject = HasComponent<BlockObject>();
      _physicalObjects = GetComponentsAllocating<IPhysicalObject>();
    }

    public void InitializeEntity() {
      if (!_isBlockObject) {
        Register();
      }
    }

    public void OnEnterFinishedState() {
      if (_isBlockObject) {
        Register();
      }
    }

    public void OnExitFinishedState() {
      if (_isBlockObject) {
        Unregister();
      }
    }

    public void DeleteEntity() {
      if (!_isBlockObject) {
        Unregister();
      }
    }

    private void Register() {
      foreach (var physicalObject in _physicalObjects) {
        _physicalObjectRegistry.Register(physicalObject);
      }
    }

    private void Unregister() {
      foreach (var physicalObject in _physicalObjects) {
        _physicalObjectRegistry.Unregister(physicalObject);
      }
    }

  }
}