using HarmonyLib;
using JetBrains.Annotations;
using SecondShift.Core;
using System.Linq;
using Timberborn.DwellingSystem;

namespace SecondShift.Patching {
  [HarmonyPatch(typeof(Dweller), nameof(Dweller.IsLookingForBetterHome))]
  [UsedImplicitly]
  internal class DwellerPatcher {

    [UsedImplicitly]
    public static bool Prefix(Dweller __instance, ref bool __result) {
      if (__instance.GetComponent<TwoShiftsWorkingHours>() is { } twoShiftsWorkingHours
          && __instance.HasHome
          && __instance.Home.AdultSlots == 2
          && __instance.Home.FreeAdultSlots == 0) {
        var otherWorkingHours = __instance.Home.AdultDwellers
            .FirstOrDefault(d => d != __instance)?.GetComponent<TwoShiftsWorkingHours>();
        if (otherWorkingHours is not null
            && !twoShiftsWorkingHours.GoesToSleepTogetherWith(otherWorkingHours)) {
          __result = true;
          return false;
        }
      }
      return true;
    }

  }
}