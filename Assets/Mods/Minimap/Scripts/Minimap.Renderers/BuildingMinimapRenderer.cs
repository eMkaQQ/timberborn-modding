﻿using Bindito.Core;
using Minimap.Core;
using Minimap.Settings;
using Timberborn.BaseComponentSystem;
using Timberborn.GameDistricts;
using Timberborn.PathSystem;
using Timberborn.Wonders;
using UnityEngine;

namespace Minimap.Renderers {
  internal class BuildingMinimapRenderer : BaseComponent,
                                           IMinimapBlockObjectRenderer {

    private MinimapColorSettings _minimapColorSettings;
    private bool _isPath;

    [Inject]
    public void InjectDependencies(MinimapColorSettings minimapColorSettings) {
      _minimapColorSettings = minimapColorSettings;
    }

    public void Awake() {
      _isPath = GetComponentFast<PathSpec>()
                && !GetComponentFast<DistrictCenter>()
                && !GetComponentFast<Wonder>();
    }

    public Color GetColor() {
      return _isPath
          ? _minimapColorSettings.PathColor.Color
          : _minimapColorSettings.BuildingColor.Color;
    }

  }
}