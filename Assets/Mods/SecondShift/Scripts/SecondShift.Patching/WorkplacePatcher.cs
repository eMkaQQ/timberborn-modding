using HarmonyLib;
using JetBrains.Annotations;
using SecondShift.Core;
using System;
using Timberborn.WorkSystem;
using Timberborn.WorldPersistence;

namespace SecondShift.Patching {
  [HarmonyPatch(typeof(Workplace))]
  [UsedImplicitly]
  internal class WorkplacePatcher {

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Workplace.MaxWorkers), MethodType.Getter)]
    [UsedImplicitly]
    public static bool MaxWorkersPrefix(Workplace __instance, ref int __result) {
      __result = __instance.GetComponent<TwoShiftsWorkplace>().TwoShiftsEnabled
          ? __instance._workplaceSpec.MaxWorkers * 2
          : __instance._workplaceSpec.MaxWorkers;
      return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Workplace.Load))]
    [UsedImplicitly]
    public static bool LoadPrefix(Workplace __instance, IEntityLoader entityLoader) {
      if (entityLoader.TryGetComponent(Workplace.WorkplaceKey, out var workplace)) {
        var twoShiftsWorkplace = __instance.GetComponent<TwoShiftsWorkplace>();
        var desiredWorkers = workplace.Get(Workplace.DesiredWorkersKey);
        if (twoShiftsWorkplace.WasLoaded) {
          __instance.SetDesiredWorkers(Math.Min(desiredWorkers, __instance.MaxWorkers));
        } else {
          twoShiftsWorkplace.SetDesiredWorkersToLoad(desiredWorkers);
        }
      }
      return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Workplace.IncreaseDesiredWorkers))]
    [UsedImplicitly]
    public static bool IncreaseDesiredWorkersPrefix(Workplace __instance) {
      IncreaseDesiredWorkers(__instance);
      var twoShiftsWorkplace = __instance.GetComponent<TwoShiftsWorkplace>();
      if (twoShiftsWorkplace.TwoShiftsEnabled) {
        IncreaseDesiredWorkers(__instance);
      }
      return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Workplace.DecreaseDesiredWorkers))]
    [UsedImplicitly]
    public static bool DecreaseDesiredWorkersPrefix(Workplace __instance) {
      var twoShiftsWorkplace = __instance.GetComponent<TwoShiftsWorkplace>();
      if (twoShiftsWorkplace.TwoShiftsEnabled) {
        if (__instance.DesiredWorkers > 2) {
          DecreaseDesiredWorkers(__instance);
          DecreaseDesiredWorkers(__instance);
        }
      } else if (__instance.DesiredWorkers > 1) {
        DecreaseDesiredWorkers(__instance);
      }
      return false;
    }

    private static void IncreaseDesiredWorkers(Workplace workplace) {
      if (workplace.DesiredWorkers < workplace.MaxWorkers) {
        workplace.SetDesiredWorkers(workplace.DesiredWorkers + 1);
      }
    }

    private static void DecreaseDesiredWorkers(Workplace workplace) {
      workplace.SetDesiredWorkers(workplace.DesiredWorkers - 1);
    }

  }
}