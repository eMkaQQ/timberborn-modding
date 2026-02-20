using Timberborn.SingletonSystem;
using UnityEngine;

namespace TimberPhysics.Core {
  internal class PhysicsSimulator : IUpdatableSingleton {

    private static readonly float FixedDeltaTime = 0.02f;
    private readonly PhysicalObjectRegistry _physicalObjectRegistry;
    private float _timer;

    public PhysicsSimulator(PhysicalObjectRegistry physicalObjectRegistry) {
      _physicalObjectRegistry = physicalObjectRegistry;
    }

    public void UpdateSingleton() {
      if (Physics.simulationMode == SimulationMode.Script) {
        _timer += Time.deltaTime;

        while (_timer >= FixedDeltaTime) {
          _timer -= FixedDeltaTime;
          _physicalObjectRegistry.StepAll(FixedDeltaTime);
          Physics.Simulate(FixedDeltaTime);
        }
      }
    }

  }
}