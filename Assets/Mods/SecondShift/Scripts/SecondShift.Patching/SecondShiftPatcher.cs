using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.ModManagerScene;

namespace SecondShift.Patching {
  [UsedImplicitly]
  internal class SecondShiftPatcher : IModStarter {

    public void StartMod(IModEnvironment modEnvironment) {
      new Harmony("eMka.SecondShift").PatchAll();
    }

  }
}