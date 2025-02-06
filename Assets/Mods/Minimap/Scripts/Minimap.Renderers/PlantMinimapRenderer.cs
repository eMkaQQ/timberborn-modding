using Bindito.Core;
using Minimap.Core;
using Minimap.Settings;
using Timberborn.BaseComponentSystem;
using Timberborn.NaturalResourcesLifecycle;
using Timberborn.NaturalResourcesMoisture;
using UnityEngine;

namespace Minimap.Renderers {
  internal class PlantMinimapRenderer : BaseComponent,
                                        IMinimapBlockObjectRenderer {

    private MinimapColorSettings _minimapColorSettings;
    private LivingNaturalResource _livingNaturalResource;
    private bool _isWaterPlant;

    [Inject]
    public void InjectDependencies(MinimapColorSettings minimapColorSettings) {
      _minimapColorSettings = minimapColorSettings;
    }

    public void Awake() {
      _livingNaturalResource = GetComponentFast<LivingNaturalResource>();
      _isWaterPlant =
          GetComponentFast<FloodableNaturalResourceSpec>() is { MaxWaterHeight: > 0 } or
                                                              { MinWaterHeight: > 0 };
    }

    public Color GetColor() {
      return _livingNaturalResource.IsDead
          ? _minimapColorSettings.DeadPlantColor.Color
          : _isWaterPlant
              ? _minimapColorSettings.WaterPlantColor.Color
              : _minimapColorSettings.PlantColor.Color;
    }

  }
}