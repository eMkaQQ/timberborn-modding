using System.Collections.Generic;
using System.Collections.Immutable;
using Timberborn.Goods;

namespace Riverborne.Core {
  public class RaftDispatch {

    public string Name { get; }
    public ImmutableArray<GoodAmount> Cargo { get; }
    public float Interval { get; }
    public float LastDispatchTime { get; private set; }
    public bool IsPaused { get; private set; }

    public RaftDispatch(string name,
                        IEnumerable<GoodAmount> cargo,
                        float interval,
                        float lastDispatchTime,
                        bool isPaused) {
      Name = name;
      Cargo = cargo.ToImmutableArray();
      Interval = interval;
      LastDispatchTime = lastDispatchTime;
      IsPaused = isPaused;
    }

    public float DayTimeInterval => Interval / 24f;

    public void UpdateLastDispatchTime(float time) {
      LastDispatchTime = time;
    }

    public void TogglePaused() {
      IsPaused = !IsPaused;
    }

  }
}