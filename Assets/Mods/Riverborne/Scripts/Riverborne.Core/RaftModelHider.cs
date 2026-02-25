using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.LevelVisibilitySystem;
using TimberPhysics.Core;
using UnityEngine;

namespace Riverborne.Core {
  internal class RaftModelHider : BaseComponent,
                                  IAwakableComponent,
                                  IUpdatableComponent {

    private readonly ILevelVisibilityService _levelVisibilityService;
    private RigidbodyAttacher _rigidbodyAttacher;
    private GameObject _model;
    private RigidbodyConstraints? _originalConstraints;

    public RaftModelHider(ILevelVisibilityService levelVisibilityService) {
      _levelVisibilityService = levelVisibilityService;
    }

    public void Awake() {
      _rigidbodyAttacher = GetComponent<RigidbodyAttacher>();
      var spec = GetComponent<RaftModelHiderSpec>();
      _model = GameObject.FindChild(spec.ModelName).gameObject;
    }

    public void Update() {
      if (_levelVisibilityService.LevelIsAtMax) {
        Show();
      } else {
        if (Transform.position.y > _levelVisibilityService.MaxVisibleLevel) {
          Hide();
        } else {
          Show();
        }
      }
    }

    private void Show() {
      if (_originalConstraints.HasValue) {
        _rigidbodyAttacher.Rigidbody.constraints = _originalConstraints.Value;
        _originalConstraints = null;
        _model.SetActive(true);
      }
    }

    private void Hide() {
      if (_originalConstraints == null) {
        _originalConstraints = _rigidbodyAttacher.Rigidbody.constraints;
        _rigidbodyAttacher.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _model.SetActive(false);
      }
    }

  }
}