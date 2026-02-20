using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace TimberPhysics.Layers {
  public class PhysicsLayerRegistry : IUnloadableSingleton {

    public static readonly int DefaultLayersIndex = 5;
    private static readonly int MaxLayerIndex = 31;
    private readonly List<PhysicsLayer> _physicsLayers = new();
    private readonly List<IgnoredCollision> _ignoredCollisions = new();

    public void AddLayer(string layerName, IEnumerable<string> ignoredLayers) {
      if (_physicsLayers.Any(l => l.Name == layerName)) {
        Debug.LogWarning($"Layer {layerName} already exists. Ignoring...");
        return;
      }

      var index = MaxLayerIndex - _physicsLayers.Count;
      if (index <= DefaultLayersIndex) {
        ThrowTooManyLayersException();
      }
      var newLayer = new PhysicsLayer(layerName, index, ignoredLayers);

      UpdateLayerCollisions(newLayer);
      _physicsLayers.Add(newLayer);
    }

    public void AssignGameObjectToLayer(GameObject gameObject, string layerName) {
      var layer = _physicsLayers.SingleOrDefault(l => l.Name == layerName);
      if (layer == null) {
        AddLayer(layerName, Enumerable.Empty<string>());
        layer = _physicsLayers.Single(l => l.Name == layerName);
      }
      AssignGameObjectToLayerRecursively(gameObject, layer.Index);
    }

    public bool TryGetLayerIndex(string layerName, out int layerIndex) {
      for (var index = 0; index < _physicsLayers.Count; index++) {
        if (_physicsLayers[index].Name == layerName) {
          layerIndex = _physicsLayers[index].Index;
          return true;
        }
      }
      layerIndex = -1;
      return false;
    }

    public void Unload() {
      foreach (var ignoredCollision in _ignoredCollisions) {
        Physics.IgnoreLayerCollision(ignoredCollision.Layer1, ignoredCollision.Layer2, false);
      }
      _physicsLayers.Clear();
      _ignoredCollisions.Clear();
    }

    private void ThrowTooManyLayersException() {
      var maxCustomLayers = MaxLayerIndex - DefaultLayersIndex;
      throw new InvalidOperationException(
          $"Cannot add more than {maxCustomLayers} layers, maximum is {MaxLayerIndex} "
          + "including default layers. Added layers: "
          + string.Join(", ", _physicsLayers.Select(layer => layer.Name)));
    }

    private void UpdateLayerCollisions(PhysicsLayer newLayer) {
      if (newLayer.IgnoredLayers.Contains(newLayer.Name)) {
        IgnoreLayerCollision(newLayer.Index, newLayer.Index);
      }
      foreach (var physicsLayer in _physicsLayers) {
        foreach (var ignoredLayer in physicsLayer.IgnoredLayers) {
          if (ignoredLayer == newLayer.Name) {
            IgnoreLayerCollision(physicsLayer.Index, newLayer.Index);
          }
        }
        foreach (var newIgnoredLayer in newLayer.IgnoredLayers) {
          if (newIgnoredLayer == physicsLayer.Name) {
            IgnoreLayerCollision(newLayer.Index, physicsLayer.Index);
          }
        }
      }
    }

    private void IgnoreLayerCollision(int layer1, int layer2) {
      Physics.IgnoreLayerCollision(layer1, layer2);
      _ignoredCollisions.Add(new(layer1, layer2));
    }

    private static void AssignGameObjectToLayerRecursively(GameObject gameObject, int layerIndex) {
      gameObject.layer = layerIndex;
      foreach (Transform child in gameObject.transform) {
        AssignGameObjectToLayerRecursively(child.gameObject, layerIndex);
      }
    }

    private class PhysicsLayer {

      public string Name { get; }
      public int Index { get; }
      public ImmutableArray<string> IgnoredLayers { get; }

      public PhysicsLayer(string name,
                          int index,
                          IEnumerable<string> ignoredLayers) {
        Name = name;
        Index = index;
        IgnoredLayers = ignoredLayers.ToImmutableArray();
      }

    }

    private struct IgnoredCollision {

      public int Layer1 { get; }
      public int Layer2 { get; }

      public IgnoredCollision(int layer1,
                              int layer2) {
        Layer1 = layer1;
        Layer2 = layer2;
      }

    }

  }
}