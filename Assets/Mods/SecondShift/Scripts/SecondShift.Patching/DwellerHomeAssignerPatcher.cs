using HarmonyLib;
using JetBrains.Annotations;
using SecondShift.Core;
using System.Collections.Generic;
using System.Linq;
using Timberborn.Beavers;
using Timberborn.DwellingSystem;

namespace SecondShift.Patching {
  [HarmonyPatch(typeof(DwellerHomeAssigner), nameof(DwellerHomeAssigner.AssignDweller))]
  [UsedImplicitly]
  internal class DwellerHomeAssignerPatcher {

    [UsedImplicitly]
    public static bool Prefix(AutoAssignableDwelling dwelling,
                              IEnumerable<Beaver> primaryBeavers,
                              IEnumerable<Beaver> secondaryBeavers,
                              ref bool __result) {
      var shouldTryNormalAssignment = true;
      if (dwelling._dwelling.AdultSlots == 2 && dwelling._dwelling.FreeAdultSlots == 1) {
        var beavers = primaryBeavers.Concat(secondaryBeavers);
        foreach (var beaver in beavers) {
          if (beaver.GetComponent<TwoShiftsWorkingHours>() is { } twoShiftsWorkingHours) {
            var currentDweller = dwelling._dwelling.AdultDwellers.FirstOrDefault();
            if (currentDweller?.GetComponent<TwoShiftsWorkingHours>() is { } otherWorkingHours) {
              var dweller = beaver.GetComponent<Dweller>();
              if (twoShiftsWorkingHours.GoesToSleepTogetherWith(otherWorkingHours)) {
                if (dweller.IsLookingForBetterHome() && dwelling.CanAssignDweller(dweller)) {
                  dwelling.AssignDweller(dweller);
                  __result = true;
                  return false;
                }
              } else if (dweller.HasHome) {
                __result = false;
                shouldTryNormalAssignment = false;
              }
            }
          }
        }
      }
      return shouldTryNormalAssignment;
    }

  }
}