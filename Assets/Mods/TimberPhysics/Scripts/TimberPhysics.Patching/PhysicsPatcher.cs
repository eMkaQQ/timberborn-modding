using HarmonyLib;
using JetBrains.Annotations;
using System.Reflection;
using Timberborn.Rendering;
using TimberPhysics.Terrain;
using UnityEngine;

namespace TimberPhysics.Patching {
  [UsedImplicitly]
  [HarmonyPatch]
  internal class PhysicsPatcher {

    [UsedImplicitly]
    private static MethodBase TargetMethod() {
      // Physics.Raycast(Ray ray, out RaycastHit hitInfo)
      return AccessTools.Method(
          typeof(Physics),
          nameof(Physics.Raycast),
          new[] { typeof(Ray), typeof(RaycastHit).MakeByRefType() }
      );
    }

    [HarmonyPrefix]
    [UsedImplicitly]
    private static bool Prefix(Ray ray, ref RaycastHit hitInfo, ref bool __result) {
      var excludedLayers = 0;

      var terrainLayer = TerrainColliderService.TerrainLayerIndex;
      if (terrainLayer.HasValue) {
        excludedLayers |= 1 << terrainLayer.Value;
      }

      var ignoreLayer = Layers.IgnoreRaycastMask;
      if (ignoreLayer >= 0) {
        excludedLayers |= 1 << ignoreLayer;
      }

      var mask = ~excludedLayers;

      __result = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask);
      return false;
    }

  }
}