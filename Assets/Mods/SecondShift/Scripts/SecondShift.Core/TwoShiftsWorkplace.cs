using System;
using Timberborn.BaseComponentSystem;
using Timberborn.DuplicationSystem;
using Timberborn.EntitySystem;
using Timberborn.Persistence;
using Timberborn.WorkerTypesUI;
using Timberborn.WorkSystem;
using Timberborn.WorldPersistence;

namespace SecondShift.Core {
  public class TwoShiftsWorkplace : BaseComponent,
                                    IAwakableComponent,
                                    IPersistentEntity,
                                    IInitializableEntity,
                                    IDuplicable<TwoShiftsWorkplace> {

    private static readonly ComponentKey TwoShiftsWorkplaceKey = new("TwoShiftsWorkplace");
    private static readonly PropertyKey<bool> TwoShiftsEnabledKey = new("TwoShiftsEnabled");
    public event EventHandler TwoShiftsEnableStateChanged;
    public bool TwoShiftsEnabled { get; private set; }
    public bool CanBeEnabled { get; private set; } = true;
    public bool WasLoaded { get; private set; }
    private Workplace _workplace;
    private WorkplaceWorkerType _workplaceWorkerType;
    private int? _desiredWorkersToLoad;

    public void Awake() {
      _workplace = GetComponent<Workplace>();
      _workplaceWorkerType = GetComponent<WorkplaceWorkerType>();
      _workplaceWorkerType.WorkerTypeChanged += (_, _) => UpdateCanBeEnabled();
    }

    public void InitializeEntity() {
      UpdateCanBeEnabled();
    }

    public void Save(IEntitySaver entitySaver) {
      if (TwoShiftsEnabled) {
        var component = entitySaver.GetComponent(TwoShiftsWorkplaceKey);
        component.Set(TwoShiftsEnabledKey, true);
      }
    }

    public void Load(IEntityLoader entityLoader) {
      if (entityLoader.TryGetComponent(TwoShiftsWorkplaceKey, out var component)) {
        SetTwoShifts(component.Get(TwoShiftsEnabledKey), true);
      }
      if (_desiredWorkersToLoad.HasValue) {
        _workplace.SetDesiredWorkers(Math.Min(_desiredWorkersToLoad.Value, _workplace.MaxWorkers));
      }
      WasLoaded = true;
    }

    public void SetDesiredWorkersToLoad(int desiredWorkers) {
      _desiredWorkersToLoad = desiredWorkers;
    }

    public void DuplicateFrom(TwoShiftsWorkplace source) {
      if (source.TwoShiftsEnabled != TwoShiftsEnabled) {
        SetTwoShifts(source.TwoShiftsEnabled, true);
      }
    }

    public void EnableTwoShifts() {
      SetTwoShifts(true);
    }

    public void DisableTwoShifts() {
      SetTwoShifts(false);
    }

    private void UpdateCanBeEnabled() {
      CanBeEnabled = _workplaceWorkerType.WorkerType != WorkerTypeHelper.BotWorkerType;
      if (TwoShiftsEnabled && !CanBeEnabled) {
        SetTwoShifts(false);
      }
    }

    private void SetTwoShifts(bool enabled, bool isLoading = false) {
      if (TwoShiftsEnabled == enabled) {
        return;
      }
      if (!TwoShiftsEnabled && !CanBeEnabled) {
        return;
      }
      TwoShiftsEnabled = enabled;
      if (!isLoading) {
        if (TwoShiftsEnabled) {
          _workplace.SetDesiredWorkers(_workplace.DesiredWorkers * 2);
        } else {
          var workersToUnassign = _workplace.DesiredWorkers / 2;
          _workplace.SetDesiredWorkers(_workplace.DesiredWorkers / 2);
          for (var i = 0; i < workersToUnassign && _workplace.Overstaffed; i++) {
            _workplace.UnassignWorkerIfNonworking();
          }
        }
      }
      TwoShiftsEnableStateChanged?.Invoke(this, EventArgs.Empty);
    }

  }
}