using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.WorkSystemUI;

namespace SecondShift.Patching {
  [HarmonyPatch(typeof(WorkplaceFragment))]
  [UsedImplicitly]
  internal static class WorkplaceFragmentPatcher {

    [HarmonyPrefix]
    [HarmonyPatch(nameof(WorkplaceFragment.ShowFragment))]
    [UsedImplicitly]
    public static bool ShowFragmentPrefix() {
      return false;
    }

  }
}