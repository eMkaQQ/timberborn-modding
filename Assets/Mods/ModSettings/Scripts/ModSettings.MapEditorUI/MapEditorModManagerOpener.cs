using Timberborn.CoreUI;
using Timberborn.MainMenuModdingUI;
using Timberborn.MapEditorUI;
using Timberborn.Options;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace ModSettings.MapEditorUI {
  internal class MapEditorModManagerOpener : ILoadableSingleton {

    private readonly IOptionsBox _optionsBox;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModManagerBox _modManagerBox;

    public MapEditorModManagerOpener(IOptionsBox optionsBox,
                                     VisualElementLoader visualElementLoader,
                                     ModManagerBox modManagerBox) {
      _optionsBox = optionsBox;
      _visualElementLoader = visualElementLoader;
      _modManagerBox = modManagerBox;
    }

    public void Load() {
      var modManagerButton = _visualElementLoader.LoadVisualElement("ModSettings/ModManagerButton");
      modManagerButton.RegisterCallback<ClickEvent>(_ => _modManagerBox.Open());
      var mapEditorOptionsBox = (MapEditorOptionsBox) _optionsBox;
      mapEditorOptionsBox._root.Q<VisualElement>("OptionsBox").Insert(6, modManagerButton);
    }

  }
}