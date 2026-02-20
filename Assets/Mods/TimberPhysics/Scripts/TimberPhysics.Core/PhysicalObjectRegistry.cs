using System.Collections.Generic;

namespace TimberPhysics.Core {
  public class PhysicalObjectRegistry {

    private readonly List<IPhysicalObject> _physicalObjects = new();
    private readonly List<IPhysicalObject> _iterationCache = new();

    public void Register(IPhysicalObject physicalObject) {
      if (physicalObject == null) {
        throw new System.ArgumentNullException(nameof(physicalObject));
      }
      _physicalObjects.Add(physicalObject);
    }

    public void Unregister(IPhysicalObject physicalObject) {
      if (physicalObject == null) {
        throw new System.ArgumentNullException(nameof(physicalObject));
      }
      _physicalObjects.Remove(physicalObject);
    }

    internal void StepAll(float deltaTime) {
      _iterationCache.AddRange(_physicalObjects);
      foreach (var physicObject in _iterationCache) {
        physicObject.PhysicsStep(deltaTime);
      }
      _iterationCache.Clear();
    }

  }
}