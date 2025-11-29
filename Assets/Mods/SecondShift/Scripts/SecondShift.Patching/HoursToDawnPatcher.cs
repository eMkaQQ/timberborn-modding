using HarmonyLib;
using JetBrains.Annotations;
using SecondShift.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Timberborn.BeaverBehavior;
using Timberborn.NeedBehaviorSystem;
using Timberborn.SleepSystem;
using Timberborn.TimeSystem;

namespace SecondShift.Patching {
  [HarmonyPatch]
  [UsedImplicitly]
  internal static class HoursToDawnPatcher {

    private static readonly MethodInfo HoursToNextStartOf =
        AccessTools.Method(typeof(IDayNightCycle), nameof(IDayNightCycle.HoursToNextStartOf),
                           new[] { typeof(TimeOfDay) });

    public static float Adjust(float original, Sleeper sleeper) {
      if (sleeper.GetComponent<TwoShiftsWorkingHours>() is { } twoShiftsWorkingHours
          && twoShiftsWorkingHours.IsSecondShiftWorker()) {
        return DayNightCycle.HoursToNextHour(12, sleeper._dayNightCycle.HoursPassedToday);
      }

      return original;
    }

    [UsedImplicitly]
    private static IEnumerable<MethodBase> TargetMethods() {
      yield return AccessTools.Method(typeof(Sleeper),
                                      nameof(Sleeper.CalculateWakeUpTimestamp));

      yield return AccessTools.Method(typeof(BeaverNeedBehaviorPicker),
                                      nameof(BeaverNeedBehaviorPicker.GetBestNeedBehavior),
                                      new[] { typeof(NeedFilter) });
    }

    [UsedImplicitly]
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions) {
      var code = new List<CodeInstruction>(instructions);
      var adjustMethod = AccessTools.Method(typeof(HoursToDawnPatcher), nameof(Adjust));

      for (var i = 0; i < code.Count; i++) {
        if (code[i].Calls(HoursToNextStartOf)) {
          code.InsertRange(i + 1, new[] {
              new CodeInstruction(OpCodes.Ldarg_0),
              new CodeInstruction(OpCodes.Call, adjustMethod)
          });
          break;
        }
      }

      return code;
    }

  }
}