using System;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftLauncher : MonoBehaviour {

    public event EventHandler<Collider> CollisionEntered;
    public event EventHandler<Collider> CollisionExited;

    private void OnTriggerEnter(Collider other) {
      CollisionEntered?.Invoke(this, other);
    }

    private void OnTriggerExit(Collider other) {
      CollisionExited?.Invoke(this, other);
    }

  }
}