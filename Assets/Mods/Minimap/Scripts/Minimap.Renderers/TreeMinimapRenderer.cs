using Bindito.Core;
using Minimap.Core;
using Minimap.Settings;
using Timberborn.BaseComponentSystem;
using Timberborn.NaturalResourcesLifecycle;
using UnityEngine;

namespace Minimap.Renderers {
  internal class TreeMinimapRenderer : BaseComponent,
                                       IMinimapBlockObjectRenderer {

    private MinimapColorSettings _minimapColorSettings;
    private LivingNaturalResource _livingNaturalResource;

    [Inject]
    public void InjectDependencies(MinimapColorSettings minimapColorSettings) {
      _minimapColorSettings = minimapColorSettings;
    }

    public void Awake() {
      _livingNaturalResource = GetComponentFast<LivingNaturalResource>();
    }

    public Color GetColor() {
      return _livingNaturalResource.IsDead
          ? _minimapColorSettings.DeadTreeColor.Color
          : _minimapColorSettings.TreeColor.Color;
    }

  }
}