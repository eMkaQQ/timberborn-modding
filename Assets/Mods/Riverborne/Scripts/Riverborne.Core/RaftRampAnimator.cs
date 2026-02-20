using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.MechanicalSystem;
using Timberborn.SingletonSystem;
using Timberborn.TickSystem;
using Timberborn.TimbermeshAnimations;
using Timberborn.TimeSystem;

namespace Riverborne.Core {
  internal class RaftRampAnimator : TickableComponent,
                                    IAwakableComponent,
                                    IFinishedStateListener {

    private readonly NonlinearAnimationManager _nonlinearAnimationManager;
    private readonly EventBus _eventBus;
    private RaftRampSpec _raftRampSpec;
    private MechanicalNode _mechanicalNode;
    private IAnimator _animator;

    public RaftRampAnimator(NonlinearAnimationManager nonlinearAnimationManager,
                            EventBus eventBus) {
      _nonlinearAnimationManager = nonlinearAnimationManager;
      _eventBus = eventBus;
    }

    public void Awake() {
      _raftRampSpec = GetComponent<RaftRampSpec>();
      _mechanicalNode = GetComponent<MechanicalNode>();
      _animator = GameObject.GetComponentInChildren<IAnimator>(true);
      DisableComponent();
    }

    public override void Tick() {
      UpdateAnimation();
    }

    public void OnEnterFinishedState() {
      EnableComponent();
      _eventBus.Register(this);
    }

    public void OnExitFinishedState() {
      DisableComponent();
      _eventBus.Unregister(this);
    }

    [OnEvent]
    public void OnCurrentSpeedChanged(CurrentSpeedChangedEvent currentSpeedChangedEvent) {
      UpdateAnimation();
    }

    private void UpdateAnimation() {
      if (_animator != null) {
        if (CanAnimate()) {
          _animator.Enabled = true;
          _animator.Speed = _mechanicalNode.PowerEfficiency
                            * _raftRampSpec.AnimationSpeed
                            * _nonlinearAnimationManager.SpeedMultiplier;
        } else {
          _animator.Enabled = false;
        }
      }
    }

    private bool CanAnimate() {
      return _mechanicalNode.ActiveAndPowered
             && (_mechanicalNode.IsConsuming
                 || _mechanicalNode.IsGenerator
                 || _mechanicalNode.IsShaft && _mechanicalNode.Powered);
    }

  }
}