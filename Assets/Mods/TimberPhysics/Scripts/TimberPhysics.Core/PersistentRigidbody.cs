using Timberborn.BaseComponentSystem;
using Timberborn.Persistence;
using Timberborn.WorldPersistence;
using UnityEngine;

namespace TimberPhysics.Core {
  internal class PersistentRigidbody : BaseComponent,
                                       IAwakableComponent,
                                       IPersistentEntity {

    private static readonly ComponentKey PersistentRigidbodyKey = new("PersistentRigidbody");
    private static readonly PropertyKey<Vector3> PositionKey = new("Position");
    private static readonly PropertyKey<Quaternion> RotationKey = new("Rotation");
    private static readonly PropertyKey<Vector3> LinearVelocityKey = new("LinearVelocity");
    private static readonly PropertyKey<Vector3> AngularVelocityKey = new("AngularVelocity");
    private static readonly PropertyKey<float> LinearDampingKey = new("LinearDamping");
    private static readonly PropertyKey<float> AngularDampingKey = new("AngularDamping");
    private RigidbodyAttacher _rigidbodyAttacher;

    public void Awake() {
      _rigidbodyAttacher = GetComponent<RigidbodyAttacher>();
    }

    public void Save(IEntitySaver entitySaver) {
      var persistentRigidbody = entitySaver.GetComponent(PersistentRigidbodyKey);
      persistentRigidbody.Set(PositionKey, GameObject.transform.position);
      persistentRigidbody.Set(RotationKey, GameObject.transform.rotation);
      persistentRigidbody.Set(LinearVelocityKey, _rigidbodyAttacher.Rigidbody.linearVelocity);
      persistentRigidbody.Set(AngularVelocityKey, _rigidbodyAttacher.Rigidbody.angularVelocity);
      persistentRigidbody.Set(LinearDampingKey, _rigidbodyAttacher.Rigidbody.linearDamping);
      persistentRigidbody.Set(AngularDampingKey, _rigidbodyAttacher.Rigidbody.angularDamping);
    }

    public void Load(IEntityLoader entityLoader) {
      var persistentRigidbody = entityLoader.GetComponent(PersistentRigidbodyKey);
      GameObject.transform.position = persistentRigidbody.Get(PositionKey);
      GameObject.transform.rotation = persistentRigidbody.Get(RotationKey);
      _rigidbodyAttacher.Rigidbody.linearVelocity = persistentRigidbody.Get(LinearVelocityKey);
      _rigidbodyAttacher.Rigidbody.angularVelocity = persistentRigidbody.Get(AngularVelocityKey);
      _rigidbodyAttacher.Rigidbody.linearDamping = persistentRigidbody.Get(LinearDampingKey);
      _rigidbodyAttacher.Rigidbody.angularDamping = persistentRigidbody.Get(AngularDampingKey);
    }

  }
}