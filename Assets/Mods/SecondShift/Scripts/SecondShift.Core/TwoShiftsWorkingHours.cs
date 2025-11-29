using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.TimeSystem;
using Timberborn.WorkSystem;

namespace SecondShift.Core {
  public class TwoShiftsWorkingHours : BaseComponent,
                                       IAwakableComponent {

    private readonly IDayNightCycle _dayNightCycle;
    private readonly WorkingHoursManager _workingHoursManager;
    private Worker _worker;
    private WorkerWorkingHours _workerWorkingHours;

    public TwoShiftsWorkingHours(IDayNightCycle dayNightCycle,
                                 WorkingHoursManager workingHoursManager) {
      _dayNightCycle = dayNightCycle;
      _workingHoursManager = workingHoursManager;
    }

    public void Awake() {
      _worker = GetComponent<Worker>();
      _workerWorkingHours = GetComponent<WorkerWorkingHours>();
    }

    public bool AreWorkingHours() {
      if (_workerWorkingHours._ignoreWorkingHours) {
        return true;
      }
      if (IsTwoShiftsWorkplace()) {
        return AreTwoShiftsWorkingHours();
      }
      return _workingHoursManager.AreWorkingHours;
    }

    public bool IsSecondShiftWorker() {
      return _worker is { Employed: true }
             && _worker.Workplace.AssignedWorkers.IndexOf(_worker) % 2 == 1;
    }

    public bool GoesToSleepTogetherWith(TwoShiftsWorkingHours other) {
      var isTwoShiftsWorkplace = IsTwoShiftsWorkplace();
      var otherIsTwoShiftsWorkplace = other.IsTwoShiftsWorkplace();

      if (isTwoShiftsWorkplace) {
        if (otherIsTwoShiftsWorkplace) {
          return IsSecondShiftWorker() == other.IsSecondShiftWorker();
        }
        return !IsSecondShiftWorker();
      }

      if (otherIsTwoShiftsWorkplace) {
        return !other.IsSecondShiftWorker();
      }

      return true;
    }

    private bool AreTwoShiftsWorkingHours() {
      return IsSecondShiftTime() && IsSecondShiftWorker()
             || !IsSecondShiftTime() && !IsSecondShiftWorker();
    }

    private bool IsTwoShiftsWorkplace() {
      return _worker.Workplace?.GetComponent<TwoShiftsWorkplace>() is { TwoShiftsEnabled: true };
    }

    private bool IsSecondShiftTime() {
      return _dayNightCycle.DayProgress > 0.5f;
    }

  }
}