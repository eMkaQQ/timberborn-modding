using HarmonyLib;
using JetBrains.Annotations;
using SecondShift.Core;
using Timberborn.PowerGeneration;

namespace SecondShift.Patching {
  [HarmonyPatch(typeof(WalkerPoweredGenerator))]
  [UsedImplicitly]
  internal class WalkerPoweredGeneratorPatcher {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(WalkerPoweredGenerator.GetBonusMultiplier))]
    [UsedImplicitly]
    private static void GetBonusMultiplierPostfix(WalkerPoweredGenerator __instance,
                                                  ref float __result) {
      if (__instance.GetComponent<TwoShiftsWorkplace>() is { TwoShiftsEnabled: true }) {
        __result *= 2f;
      }
    }

  }
}