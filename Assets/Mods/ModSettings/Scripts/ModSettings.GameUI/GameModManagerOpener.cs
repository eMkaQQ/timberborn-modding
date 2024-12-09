using Timberborn.CoreUI;
using Timberborn.MainMenuModdingUI;
using Timberborn.Options;
using Timberborn.OptionsGame;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace ModSettings.GameUI {
  internal class GameModManagerOpener : ILoadableSingleton {

    private readonly IOptionsBox _optionsBox;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModManagerBox _modManagerBox;

    public GameModManagerOpener(IOptionsBox optionsBox,
                                VisualElementLoader visualElementLoader,
                                ModManagerBox modManagerBox) {
      _optionsBox = optionsBox;
      _visualElementLoader = visualElementLoader;
      _modManagerBox = modManagerBox;
    }

    public void Load() {
      var modManagerButton = _visualElementLoader.LoadVisualElement("ModSettings/ModManagerButton");
      modManagerButton.RegisterCallback<ClickEvent>(_ => _modManagerBox.Open());
      var gameOptionsBox = (GameOptionsBox) _optionsBox;
      gameOptionsBox._root.Q<VisualElement>("OptionsBox").Insert(4, modManagerButton);
    }

  }
}