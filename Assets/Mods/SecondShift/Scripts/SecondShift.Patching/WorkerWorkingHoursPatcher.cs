using HarmonyLib;
using JetBrains.Annotations;
using SecondShift.Core;
using Timberborn.WorkSystem;

namespace SecondShift.Patching {
  [HarmonyPatch(typeof(WorkerWorkingHours), nameof(WorkerWorkingHours.AreWorkingHours),
                MethodType.Getter)]
  [UsedImplicitly]
  internal class WorkerWorkingHoursPatcher {

    [UsedImplicitly]
    public static bool Prefix(WorkerWorkingHours __instance, ref bool __result) {
      __result = __instance.GetComponent<TwoShiftsWorkingHours>().AreWorkingHours();
      return false;
    }

  }
}