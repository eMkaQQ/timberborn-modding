using ModSettings.Core;
using ModSettings.CoreUI;
using System.Collections.Generic;
using System.Reflection;
using Timberborn.CoreUI;
using Timberborn.MainMenuModdingUI;
using Timberborn.Modding;
using Timberborn.ModdingUI;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace ModSettings.MainMenuUI {
  internal class ModItemSettingsInitializer : ILoadableSingleton {

    private static readonly string ModListViewFieldName = "_modListView";
    private static readonly string ModItemsFieldName = "_modItems";
    private readonly ModSettingsBox _modSettingsBox;
    private readonly ModSettingsOwnerRegistry _modSettingsOwnerRegistry;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModManagerBox _modManagerBox;

    public ModItemSettingsInitializer(ModSettingsBox modSettingsBox,
                                      ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                      VisualElementLoader visualElementLoader,
                                      ModManagerBox modManagerBox) {
      _modSettingsBox = modSettingsBox;
      _modSettingsOwnerRegistry = modSettingsOwnerRegistry;
      _visualElementLoader = visualElementLoader;
      _modManagerBox = modManagerBox;
    }

    public void Load() {
      foreach (var createdModItem in GetCreatedModItems()) {
        Initialize(createdModItem.Value);
      }
    }

    private Dictionary<Mod, ModItem> GetCreatedModItems() {
      var modListView = GetModListView();
      var modItemsField = modListView.GetType()
          .GetField(ModItemsFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
      if (modItemsField == null) {
        throw new($"Mod items field named {ModItemsFieldName} "
                  + $"wasn't found in {modListView.GetType().Name}");
      }
      return (Dictionary<Mod, ModItem>) modItemsField.GetValue(modListView);
    }

    private ModListView GetModListView() {
      var modListViewField = _modManagerBox.GetType()
          .GetField(ModListViewFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
      if (modListViewField == null) {
        throw new($"{nameof(ModListView)} field named {ModListViewFieldName} "
                  + $"wasn't found in {_modManagerBox.GetType().Name}");
      }
      return (ModListView) modListViewField.GetValue(_modManagerBox);
    }

    private void Initialize(ModItem modItem) {
      var button = _visualElementLoader.LoadVisualElement("ModSettings/ModSettingsButton");
      modItem.Root.Add(button);
      button.RegisterCallback<AttachToPanelEvent>(
          _ => button.ToggleDisplayStyle(
              _modSettingsOwnerRegistry.HasModSettings(modItem.Mod)));
      button.RegisterCallback<ClickEvent>(_ => _modSettingsBox.Open(modItem.Mod));
    }

  }
}