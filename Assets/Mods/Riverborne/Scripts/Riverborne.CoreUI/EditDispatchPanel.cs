using Riverborne.Core;
using System;
using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.Goods;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using Timberborn.TimeSystem;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  internal class EditDispatchPanel : ILoadableSingleton,
                                     IPanelController,
                                     IUpdatableSingleton {

    public static readonly int MaxTotalWeight = 70;
    private static readonly float CooldownChangeStep = 0.1f;
    private static readonly float DefaultCooldown = 4f;
    private static readonly string DefaultNameLocKey =
        "Riverborne.EditDispatchPanel.DefaultDispatchName";
    private static readonly string TotalWeightLocKey = "Riverborne.EditDispatchPanel.TotalWeight";
    private static readonly string IntervalLocKey = "Riverborne.EditDispatchPanel.Interval";
    private static readonly string HoursShortLocKey = "Time.HoursShort";
    private static readonly string OverloadClass = "game-text--red";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly PanelStack _panelStack;
    private readonly EditDispatchPanelGoodFactory _editDispatchPanelGoodFactory;
    private readonly ILoc _loc;
    private readonly IDayNightCycle _dayNightCycle;
    private VisualElement _root;
    private Label _intervalLabel;
    private PreciseSlider _intervalSlider;
    private Label _totalWeightLabel;
    private Button _saveButton;
    private TextField _nameField;
    private readonly List<EditDispatchPanelGood> _goods = new();
    private bool _isShown;
    private Action<RaftDispatch> _callback;
    private bool _isOverloaded;

    public EditDispatchPanel(VisualElementLoader visualElementLoader,
                             PanelStack panelStack,
                             EditDispatchPanelGoodFactory editDispatchPanelGoodFactory,
                             ILoc loc,
                             IDayNightCycle dayNightCycle) {
      _visualElementLoader = visualElementLoader;
      _panelStack = panelStack;
      _editDispatchPanelGoodFactory = editDispatchPanelGoodFactory;
      _loc = loc;
      _dayNightCycle = dayNightCycle;
    }

    public void Load() {
      _root = _visualElementLoader.LoadVisualElement("Riverborne/EditDispatchPanel");
      var goodsParent = _root.Q<VisualElement>("GoodGroups");
      _goods.AddRange(_editDispatchPanelGoodFactory.CreateGoods(goodsParent));
      _saveButton = _root.Q<Button>("SaveButton");
      _saveButton.RegisterCallback<ClickEvent>(_ => Save());
      _root.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(_ => Close());
      _intervalLabel = _root.Q<Label>("IntervalLabel");
      _intervalSlider = _root.Q<PreciseSlider>("IntervalSlider");
      _intervalSlider.Initialize(UpdateIntervalLabel, CooldownChangeStep);
      _totalWeightLabel = _root.Q<Label>("TotalWeightLabel");
      _nameField = _root.Q<TextField>("Name");
      _nameField.RegisterValueChangedCallback(_ => UpdateSaveButton());
    }

    public VisualElement GetPanel() {
      return _root;
    }

    public bool OnUIConfirmed() {
      Save();
      return true;
    }

    public void OnUICancelled() {
      Close();
    }

    public void UpdateSingleton() {
      if (_isShown) {
        foreach (var editDispatchPanelGood in _goods) {
          editDispatchPanelGood.Update();
        }
        UpdateWeight();
      }
    }

    public void ShowNew(Action<RaftDispatch> callback, RaftDock raftDock) {
      foreach (var editDispatchPanelGood in _goods) {
        editDispatchPanelGood.SetAmount(0);
      }
      _intervalSlider.UpdateValuesWithoutNotify(DefaultCooldown, 24f);
      var baseName = _loc.T(DefaultNameLocKey);
      _nameField.value = raftDock.GetNextDispatchName(baseName);
      Show(callback);
    }

    public void ShowExisting(RaftDispatch raftDispatch,
                             Action<RaftDispatch> callback) {
      foreach (var editDispatchPanelGood in _goods) {
        editDispatchPanelGood.SetAmount(0);
        foreach (var goodAmount in raftDispatch.Cargo) {
          if (goodAmount.GoodId == editDispatchPanelGood.GoodId) {
            editDispatchPanelGood.SetAmount(goodAmount.Amount);
            break;
          }
        }
      }
      _intervalSlider.UpdateValuesWithoutNotify(raftDispatch.Interval, 24f);
      _nameField.value = raftDispatch.Name;
      Show(callback);
    }

    private void UpdateIntervalLabel(float interval) {
      var hours = _loc.T(HoursShortLocKey, interval.ToString("F1"));
      _intervalLabel.text = _loc.T(IntervalLocKey, hours);
    }

    private void UpdateWeight() {
      var totalWeight = 0;
      foreach (var editDispatchPanelGood in _goods) {
        totalWeight += editDispatchPanelGood.GetWeight();
      }
      _totalWeightLabel.text = _loc.T(TotalWeightLocKey, totalWeight, MaxTotalWeight);
      _isOverloaded = totalWeight > MaxTotalWeight;
      _totalWeightLabel.EnableInClassList(OverloadClass, _isOverloaded);
      UpdateSaveButton();
    }

    private void UpdateSaveButton() {
      _saveButton.SetEnabled(!_isOverloaded && !string.IsNullOrWhiteSpace(_nameField.value));
    }

    private void Show(Action<RaftDispatch> callback) {
      UpdateWeight();
      UpdateIntervalLabel(_intervalSlider.Value);
      _panelStack.PushOverlay(this);
      _callback = callback;
      _isShown = true;
    }

    private void Save() {
      _callback.Invoke(CreateRaftDispatch());
      Close();
    }

    private void Close() {
      _panelStack.Pop(this);
      _callback = null;
      _isShown = false;
    }

    private RaftDispatch CreateRaftDispatch() {
      var goodAmounts = new List<GoodAmount>();
      foreach (var editDispatchPanelGood in _goods) {
        var amount = editDispatchPanelGood.Amount;
        if (amount > 0) {
          goodAmounts.Add(new(editDispatchPanelGood.GoodId, amount));
        }
      }
      var interval = _intervalSlider.Value;
      return new(_nameField.value, goodAmounts, interval,
                 _dayNightCycle.PartialDayNumber - interval / 24f, false);
    }

  }
}