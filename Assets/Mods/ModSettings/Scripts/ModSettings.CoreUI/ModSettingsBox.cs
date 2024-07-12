using ModSettings.Core;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.Modding;
using Timberborn.SingletonSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  public class ModSettingsBox : IPanelController,
                                ILoadableSingleton {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly PanelStack _panelStack;
    private readonly ModSettingsOwnerRegistry _modSettingsOwnerRegistry;
    private readonly ImmutableArray<IModSettingElementFactory> _modSettingElementFactories;
    private readonly ILoc _loc;
    private VisualElement _root;
    private ScrollView _scrollView;

    public ModSettingsBox(VisualElementLoader visualElementLoader,
                          PanelStack panelStack,
                          ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                          IEnumerable<IModSettingElementFactory> modSettingElementFactories,
                          ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _panelStack = panelStack;
      _modSettingsOwnerRegistry = modSettingsOwnerRegistry;
      _modSettingElementFactories = modSettingElementFactories
          .OrderByDescending(factory => factory.Priority).ToImmutableArray();
      _loc = loc;
    }

    public void Load() {
      _root = _visualElementLoader.LoadVisualElement("ModSettings/ModSettingsBox");
      _scrollView = _root.Q<ScrollView>("Content");
      _root.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(_ => Close());
    }

    public void Open(Mod mod) {
      CreateSettingOwnerSections(mod);
      _panelStack.HideAndPush(this);
    }

    public VisualElement GetPanel() {
      return _root;
    }

    public bool OnUIConfirmed() {
      return false;
    }

    public void OnUICancelled() {
      Close();
    }

    private void Close() {
      _panelStack.Pop(this);
      _scrollView.Clear();
    }

    private void CreateSettingOwnerSections(Mod mod) {
      var settingOwners = _modSettingsOwnerRegistry.GetModSettingOwners(mod);
      foreach (var settingOwner in settingOwners.OrderBy(settingOwner => settingOwner.Order)) {
        CreateSettingOwnerSection(settingOwner);
      }
    }

    private void CreateSettingOwnerSection(ModSettingsOwner settingsOwner) {
      CreateHeader(settingsOwner);
      foreach (var modSetting in settingsOwner.ModSettings) {
        CreateSettingElement(modSetting);
      }
    }

    private void CreateHeader(ModSettingsOwner settingsOwner) {
      if (!string.IsNullOrEmpty(settingsOwner.HeaderLocKey)) {
        var header = _visualElementLoader.LoadVisualElement("ModSettings/ModSettingsHeader");
        ((Label) header).text = _loc.T(settingsOwner.HeaderLocKey);
        _scrollView.Add(header);
      }
    }

    private void CreateSettingElement(object modSetting) {
      foreach (var factory in _modSettingElementFactories) {
        if (factory.TryCreateElement(modSetting, _scrollView)) {
          return;
        }
      }
      Debug.LogWarning($"No factory found for mod setting {modSetting}");
    }

  }
}