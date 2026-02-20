using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.MechanicalSystem;
using TimberPhysics.Core;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftRamp : BaseComponent,
                            IAwakableComponent,
                            IPostPlacementChangeListener,
                            IPostLoadableEntity,
                            IPhysicalObject {

    private MechanicalNode _mechanicalNode;
    private RaftRampSpec _raftRampSpec;
    private readonly List<Rigidbody> _raftRigidbodies = new();
    private GameObject _launchingObject;
    private Vector3 _rampDirection;

    public void Awake() {
      _mechanicalNode = GetComponent<MechanicalNode>();
      _raftRampSpec = GetComponent<RaftRampSpec>();
      _launchingObject =
          GameObject.FindChildRecursive(_raftRampSpec.LaunchingObjectName).gameObject;
      _launchingObject.GetComponent<BoxCollider>().isTrigger = true;
      var launcher = _launchingObject.AddComponent<RaftLauncher>();
      launcher.CollisionEntered += OnCollisionEntered;
      launcher.CollisionExited += OnCollisionExited;
    }

    public void OnPostPlacementChanged() {
      _rampDirection = _launchingObject.transform.forward;
    }

    public void PostLoadEntity() {
      _rampDirection = _launchingObject.transform.forward;
    }

    public void PhysicsStep(float deltaTime) {
      if (_mechanicalNode.ActiveAndPowered) {
        for (var index = _raftRigidbodies.Count - 1; index >= 0; index--) {
          var raftRigidbody = _raftRigidbodies[index];
          if (raftRigidbody == null) {
            _raftRigidbodies.RemoveAt(index);
          } else {
            Accelerate(raftRigidbody);
          }
        }
      }
    }

    private void Accelerate(Rigidbody raftRigidbody) {
      var speedAlongRamp = Vector3.Dot(raftRigidbody.linearVelocity, _rampDirection);

      if (speedAlongRamp >= _raftRampSpec.MaxRampSpeed) {
        return;
      }

      var acceleration = _raftRampSpec.Acceleration * _mechanicalNode.PowerEfficiency;
      raftRigidbody.AddForce(_rampDirection * acceleration, ForceMode.Acceleration);
    }

    private void OnCollisionEntered(object sender, Collider e) {
      var rigidbody = e.attachedRigidbody;
      if (rigidbody != null
          && !_raftRigidbodies.Contains(rigidbody)
          && rigidbody.GetComponent<ComponentCache>().TryGetCachedComponent<Raft>(out _)) {
        _raftRigidbodies.Add(rigidbody);
      }
    }

    private void OnCollisionExited(object sender, Collider e) {
      var rigidbody = e.attachedRigidbody;
      if (rigidbody != null) {
        _raftRigidbodies.Remove(rigidbody);
      }
    }

  }
}