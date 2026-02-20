using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.TemplateAttachmentSystem;
using UnityEngine;

namespace TimberPhysics.Patching {
  [HarmonyPatch(typeof(TemplateAttachments))]
  [UsedImplicitly]
  internal class TemplateAttachmentsPatcher {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(TemplateAttachments.CreateAttachment))]
    [UsedImplicitly]
    private static void CreateAttachmentPostfix(TemplateAttachments __instance,
                                                ref TemplateAttachment __result) {
      AssignLayerRecursively(__result.GameObject, __instance.GameObject.layer);
    }

    private static void AssignLayerRecursively(GameObject gameObject, int layer) {
      gameObject.layer = layer;
      foreach (Transform child in gameObject.transform) {
        AssignLayerRecursively(child.gameObject, layer);
      }
    }

  }
}