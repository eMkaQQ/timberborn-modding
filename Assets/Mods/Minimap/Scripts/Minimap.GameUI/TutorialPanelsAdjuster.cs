using Minimap.Core;
using Timberborn.SingletonSystem;
using Timberborn.TutorialSystemUI;

namespace Minimap.GameUI {
  internal class TutorialPanelsAdjuster : ILoadableSingleton {

    private readonly MinimapTexture _minimapTexture;
    private readonly TutorialPanels _tutorialPanels;

    public TutorialPanelsAdjuster(MinimapTexture minimapTexture,
                                  TutorialPanels tutorialPanels) {
      _minimapTexture = minimapTexture;
      _tutorialPanels = tutorialPanels;
    }

    public void Load() {
      if (_minimapTexture.MinimapEnabled) {
        _tutorialPanels._root.style.bottom = 0;
        _tutorialPanels._root.style.position = UnityEngine.UIElements.Position.Relative;
      }
    }

  }
}