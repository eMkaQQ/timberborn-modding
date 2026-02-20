using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.ModManagerScene;

namespace TimberPhysics.Patching {
  [UsedImplicitly]
  internal class TimberPhysicsPatcher : IModStarter {

    public void StartMod(IModEnvironment modEnvironment) {
      new Harmony("eMka.TimberPhysics").PatchAll();
    }

  }
}