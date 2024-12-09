using ModSettings.Core;
using Timberborn.CoreUI;
using Timberborn.ModdingUI;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  internal class ModItemSettingsInitializer : IPostLoadableSingleton {

    private readonly ModSettingsBox _modSettingsBox;
    private readonly ModSettingsOwnerRegistry _modSettingsOwnerRegistry;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModListView _modListView;

    public ModItemSettingsInitializer(ModSettingsBox modSettingsBox,
                                      ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                      VisualElementLoader visualElementLoader,
                                      ModListView modListView) {
      _modSettingsBox = modSettingsBox;
      _modSettingsOwnerRegistry = modSettingsOwnerRegistry;
      _visualElementLoader = visualElementLoader;
      _modListView = modListView;
    }

    public void PostLoad() {
      foreach (var createdModItem in _modListView._modItems) {
        CreateSettingsButton(createdModItem.Value);
      }
    }

    private void CreateSettingsButton(ModItem modItem) {
      var button = _visualElementLoader.LoadVisualElement("ModSettings/ModSettingsButton");
      modItem.Root.Add(button);
      button.RegisterCallback<AttachToPanelEvent>(
          _ => button.ToggleDisplayStyle(
              _modSettingsOwnerRegistry.HasModSettings(modItem.Mod)));
      button.RegisterCallback<ClickEvent>(_ => _modSettingsBox.Open(modItem.Mod));
    }

  }
}