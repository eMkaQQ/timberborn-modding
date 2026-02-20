using Timberborn.BaseComponentSystem;
using UnityEngine;

namespace TimberPhysics.Core {
  public class RigidbodyAttacher : BaseComponent,
                                   IAwakableComponent {

    public Rigidbody Rigidbody { get; private set; }

    public void Awake() {
      var spec = GetComponent<RigidbodyAttacherSpec>();
      Rigidbody = GameObject.AddComponent<Rigidbody>();
      CopyRigidbodyValues(spec.Rigidbody.Asset, Rigidbody);
    }

    private static void CopyRigidbodyValues(Rigidbody src, Rigidbody dst) {
      dst.mass = src.mass;
      dst.linearDamping = src.linearDamping;
      dst.angularDamping = src.angularDamping;
      dst.useGravity = src.useGravity;
      dst.isKinematic = src.isKinematic;

      dst.interpolation = src.interpolation;
      dst.collisionDetectionMode = src.collisionDetectionMode;
      dst.constraints = src.constraints;

      dst.maxDepenetrationVelocity = src.maxDepenetrationVelocity;
      dst.sleepThreshold = src.sleepThreshold;
      dst.solverIterations = src.solverIterations;
      dst.solverVelocityIterations = src.solverVelocityIterations;
    }

  }
}