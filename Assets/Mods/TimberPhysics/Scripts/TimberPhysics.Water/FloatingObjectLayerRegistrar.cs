using System.Collections.Generic;
using Timberborn.SingletonSystem;
using TimberPhysics.GameLayers;
using TimberPhysics.Layers;
using TimberPhysics.WaterSettings;

namespace TimberPhysics.Water {
  public class FloatingObjectLayerRegistrar : ILoadableSingleton {

    public static readonly string LayerName = "FloatingObjects";

    private readonly PhysicsLayerRegistry _physicsLayerRegistry;
    private readonly FloatingObjectSettingOwner _floatingObjectSettingOwner;

    internal FloatingObjectLayerRegistrar(PhysicsLayerRegistry physicsLayerRegistry,
                                          FloatingObjectSettingOwner floatingObjectSettingOwner) {
      _physicsLayerRegistry = physicsLayerRegistry;
      _floatingObjectSettingOwner = floatingObjectSettingOwner;
    }

    public void Load() {
      _physicsLayerRegistry.AddLayer(LayerName, GetCollidingLayers());
    }

    private IEnumerable<string> GetCollidingLayers() {
      if (!_floatingObjectSettingOwner.FloatingObjectsCollisionSetting.Value) {
        yield return LayerName;
      }
      yield return CharactersLayerAssigner.LayerNameValue;
      yield return BlockObjectsLayerAssigner.UnfinishedLayerNameValue;
    }

  }
}