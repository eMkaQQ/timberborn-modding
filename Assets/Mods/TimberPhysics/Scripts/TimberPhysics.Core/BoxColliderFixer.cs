using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;
using UnityEngine;

namespace TimberPhysics.Core {
  internal class BoxColliderFixer : BaseComponent,
                                    IAwakableComponent,
                                    IPostLoadableEntity,
                                    IPrePlacementChangeListener,
                                    IPostPlacementChangeListener {

    private bool _wasNegativeScale;
    private bool _isFlipped;
    private BoxCollider[] _boxColliders;

    public void Awake() {
      _boxColliders = GameObject.GetComponentsInChildren<BoxCollider>(true);
    }

    public void PostLoadEntity() {
      if (IsNegativeScale() && !_isFlipped) {
        FlipBoxColliders();
      }
    }

    public void OnPrePlacementChanged() {
      _wasNegativeScale = IsNegativeScale();
    }

    public void OnPostPlacementChanged() {
      if (_wasNegativeScale != IsNegativeScale()) {
        FlipBoxColliders();
      }
    }

    private bool IsNegativeScale() {
      return GameObject.transform.localScale.x < 0f;
    }

    private void FlipBoxColliders() {
      for (var i = 0; i < _boxColliders.Length; i++) {
        var boxCollider = _boxColliders[i];
        boxCollider.size = new(
            -boxCollider.size.x,
            boxCollider.size.y,
            boxCollider.size.z
        );
      }
      _isFlipped = IsNegativeScale();
    }

  }
}