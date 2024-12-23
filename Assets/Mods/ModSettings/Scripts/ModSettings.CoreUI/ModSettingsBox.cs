using ModSettings.Core;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.Modding;
using Timberborn.SingletonSystem;
using Timberborn.TooltipSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  public class ModSettingsBox : IPanelController,
                                IUpdatableSingleton,
                                ILoadableSingleton {

    private static readonly string ResetMessageLocKey = "ModSettingsBox.ResetMessage";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly PanelStack _panelStack;
    private readonly ModSettingsOwnerRegistry _modSettingsOwnerRegistry;
    private readonly ImmutableArray<IModSettingElementFactory> _modSettingElementFactories;
    private readonly ILoc _loc;
    private readonly DialogBoxShower _dialogBoxShower;
    private readonly IModSettingsContextProvider _modSettingsContextProvider;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private VisualElement _root;
    private ScrollView _scrollView;
    private Mod _currentMod;
    private readonly List<IModSettingElement> _modSettingElements = new();

    public ModSettingsBox(VisualElementLoader visualElementLoader,
                          PanelStack panelStack,
                          ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                          IEnumerable<IModSettingElementFactory> modSettingElementFactories,
                          ILoc loc,
                          DialogBoxShower dialogBoxShower,
                          IModSettingsContextProvider modSettingsContextProvider,
                          ITooltipRegistrar tooltipRegistrar) {
      _visualElementLoader = visualElementLoader;
      _panelStack = panelStack;
      _modSettingsOwnerRegistry = modSettingsOwnerRegistry;
      _modSettingElementFactories = modSettingElementFactories
          .OrderByDescending(factory => factory.Priority).ToImmutableArray();
      _loc = loc;
      _dialogBoxShower = dialogBoxShower;
      _modSettingsContextProvider = modSettingsContextProvider;
      _tooltipRegistrar = tooltipRegistrar;
    }

    public void Load() {
      _root = _visualElementLoader.LoadVisualElement("ModSettings/ModSettingsBox");
      _scrollView = _root.Q<ScrollView>("Content");
      _root.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(_ => Close());
      _root.Q<Button>("ResetToDefault").RegisterCallback<ClickEvent>(_ => ShowResetDialog());
    }

    public void Open(Mod mod) {
      _currentMod = mod;
      CreateSettingOwnerSections(mod);
      _panelStack.HideAndPush(this);
    }

    public VisualElement GetPanel() {
      return _root;
    }

    public void UpdateSingleton() {
      if (_currentMod != null) {
        foreach (var modSettingElement in _modSettingElements) {
          modSettingElement.Root.SetEnabled(modSettingElement.ModSetting.Descriptor.IsEnabled());
        }
      }
    }

    public bool OnUIConfirmed() {
      return false;
    }

    public void OnUICancelled() {
      if (!IsInputBlocked()) {
        Close();
      }
    }

    private void Close() {
      _currentMod = null;
      _panelStack.Pop(this);
      _modSettingElements.Clear();
      _scrollView.Clear();
    }

    private void CreateSettingOwnerSections(Mod mod) {
      var settingOwners = _modSettingsOwnerRegistry.GetModSettingOwners(mod);
      foreach (var settingOwner in settingOwners.OrderBy(settingOwner => settingOwner.Order)) {
        if (settingOwner.ModSettings.Any()) {
          CreateSettingOwnerSection(settingOwner);
        }
      }
    }

    private void CreateSettingOwnerSection(ModSettingsOwner settingsOwner) {
      CreateHeader(settingsOwner);
      var parent = new VisualElement();
      _scrollView.Add(parent);
      foreach (var modSetting in settingsOwner.ModSettings) {
        CreateSettingElement(modSetting, parent);
      }
      SetSettingOwnerEnabledState(parent, settingsOwner);
    }

    private void CreateHeader(ModSettingsOwner settingsOwner) {
      if (!string.IsNullOrEmpty(settingsOwner.HeaderLocKey)) {
        var header = _visualElementLoader.LoadVisualElement("ModSettings/ModSettingsHeader");
        ((Label) header).text = _loc.T(settingsOwner.HeaderLocKey);
        _scrollView.Add(header);
      }
    }

    private void CreateSettingElement(ModSetting modSetting, VisualElement parent) {
      foreach (var factory in _modSettingElementFactories) {
        if (factory.TryCreateElement(modSetting, out var element)) {
          parent.Add(element.Root);
          _modSettingElements.Add(element);
          return;
        }
      }
      Debug.LogWarning($"No factory found for mod setting {modSetting}");
    }

    private void SetSettingOwnerEnabledState(VisualElement parent,
                                             ModSettingsOwner modSettingsOwner) {
      var enabled = IsModSettingsOwnerEnabled(modSettingsOwner);
      parent.SetEnabled(enabled);
      if (!enabled) {
        _tooltipRegistrar.RegisterLocalizable(parent, _modSettingsContextProvider.WarningLocKey);
      }
    }

    private bool IsModSettingsOwnerEnabled(ModSettingsOwner modSettingsOwner) {
      return modSettingsOwner.ChangeableOn.HasFlag(_modSettingsContextProvider.Context);
    }

    private void ShowResetDialog() {
      _dialogBoxShower.Create()
          .SetLocalizedMessage(ResetMessageLocKey)
          .SetDefaultCancelButton()
          .SetConfirmButton(ResetToDefault)
          .Show();
    }

    private void ResetToDefault() {
      var currentMod = _currentMod;
      Close();
      foreach (var settingOwner in _modSettingsOwnerRegistry.GetModSettingOwners(currentMod)) {
        if (IsModSettingsOwnerEnabled(settingOwner)) {
          settingOwner.ResetModSettings();
        }
      }
      Open(currentMod);
    }

    private bool IsInputBlocked() {
      return _modSettingElements.Any(element => element.ShouldBlockInput);
    }

  }
}