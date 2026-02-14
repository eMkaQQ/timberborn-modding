using SecondShift.Core;
using System;
using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.Bots;
using Timberborn.Characters;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.GameDistricts;
using Timberborn.InputSystemUI;
using Timberborn.Localization;
using Timberborn.PrioritySystemUI;
using Timberborn.SelectionSystem;
using Timberborn.TooltipSystem;
using Timberborn.WorkSystem;
using Timberborn.WorkSystemUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace SecondShift.CoreUI {
  internal class TwoShiftsWorkplaceFragment : IEntityPanelFragment {

    private static readonly string PriorityLabelLocKey = "Work.Workplace.DisplayName";
    private static readonly string PriorityTooltipLocKey = "Work.PriorityTitle";
    private static readonly string CurrentWorkersLocKey = "Work.CurrentWorkers";
    private static readonly string SecondShiftDisabledLocKey = "SecondShift.Disabled";
    private static readonly string IncreaseWorkersKey = "IncreaseWorkers";
    private static readonly string DecreaseWorkersKey = "DecreaseWorkers";
    private static readonly string ToggleWorkerTypeKey = "ToggleWorkerType";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly WorkerViewFactory _workerViewFactory;
    private readonly EntitySelectionService _entitySelectionService;
    private readonly ILoc _loc;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly WorkplacePriorityToggleGroupFactory _workplacePriorityToggleGroupFactory;
    private readonly BotPopulation _botPopulation;
    private readonly WorkerTypeToggleFactory _workerTypeToggleFactory;
    private readonly BindableButtonFactory _bindableButtonFactory;
    private readonly TwoShiftsWorkersToggle _twoShiftsWorkersToggle;
    private VisualElement _root;
    private VisualElement _workplaceUsers;
    private Label _text;
    private BindableButton _increase;
    private BindableButton _decrease;
    private Toggle _twoShiftsEnabledToggle;
    private WorkerTypeToggle _workerTypeToggle;
    private Workplace _workplace;
    private TwoShiftsWorkplace _twoShiftsWorkplace;
    private WorkplaceDescriber _workplaceDescriber;
    private readonly List<WorkerView> _views = new();
    private PriorityToggleGroup _priorityToggleGroup;

    public TwoShiftsWorkplaceFragment(
        VisualElementLoader visualElementLoader,
        WorkerViewFactory workerViewFactory,
        EntitySelectionService entitySelectionService,
        ILoc loc,
        ITooltipRegistrar tooltipRegistrar,
        WorkplacePriorityToggleGroupFactory workplacePriorityToggleGroupFactory,
        BotPopulation botPopulation,
        WorkerTypeToggleFactory workerTypeToggleFactory,
        BindableButtonFactory bindableButtonFactory,
        TwoShiftsWorkersToggle twoShiftsWorkersToggle) {
      _visualElementLoader = visualElementLoader;
      _workerViewFactory = workerViewFactory;
      _entitySelectionService = entitySelectionService;
      _loc = loc;
      _tooltipRegistrar = tooltipRegistrar;
      _workplacePriorityToggleGroupFactory = workplacePriorityToggleGroupFactory;
      _botPopulation = botPopulation;
      _workerTypeToggleFactory = workerTypeToggleFactory;
      _bindableButtonFactory = bindableButtonFactory;
      _twoShiftsWorkersToggle = twoShiftsWorkersToggle;
    }

    public VisualElement InitializeFragment() {
      _root = _visualElementLoader.LoadVisualElement("Game/EntityPanel/TwoShiftsWorkplaceFragment");
      _workplaceUsers = _root.Q<VisualElement>("WorkplaceUsers");
      _text = _root.Q<Label>("Text");
      _tooltipRegistrar.Register(_text, () => _workplaceDescriber.GetWorkersTooltip());
      _increase = _bindableButtonFactory.Create(_root.Q<Button>("Increase"),
                                                IncreaseWorkersKey,
                                                () => _workplace.IncreaseDesiredWorkers());
      _decrease = _bindableButtonFactory.Create(_root.Q<Button>("Decrease"),
                                                DecreaseWorkersKey,
                                                () => _workplace.DecreaseDesiredWorkers());
      var visualElement = _root.Q<VisualElement>("HeaderWrapper");
      _priorityToggleGroup =
          _workplacePriorityToggleGroupFactory.Create(visualElement,
                                                      PriorityLabelLocKey);
      _tooltipRegistrar.RegisterLocalizable(visualElement.Q<VisualElement>("TogglesWrapper"),
                                            PriorityTooltipLocKey);
      _workerTypeToggle = _workerTypeToggleFactory.CreateBindable(
          _root.Q<VisualElement>("WorkerTypeToggleWrapper"), ToggleWorkerTypeKey);
      _twoShiftsEnabledToggle = _root.Q<Toggle>("TwoShiftsToggle");
      _twoShiftsEnabledToggle.RegisterValueChangedCallback(ToggleTwoShifts);
      _twoShiftsWorkersToggle.Initialize(_root.Q<VisualElement>("TwoShiftsWorkersToggleWrapper"));
      _tooltipRegistrar.Register(_twoShiftsEnabledToggle, GetTwoShiftsTooltip);
      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity) {
      _workplace = entity.GetComponent<Workplace>();
      if (_workplace != null) {
        _workplaceDescriber = entity.GetComponent<WorkplaceDescriber>();
        _twoShiftsWorkplace = entity.GetComponent<TwoShiftsWorkplace>();
        var component = entity.GetComponent<WorkplaceWorkerType>();
        _workerTypeToggle.Show(component);
        _priorityToggleGroup.Enable(entity.GetComponent<WorkplacePriority>());
        AddEmptyViews(component);
        _increase.Bind();
        _decrease.Bind();
        _twoShiftsEnabledToggle.SetValueWithoutNotify(_twoShiftsWorkplace.TwoShiftsEnabled);
        _twoShiftsWorkplace.TwoShiftsEnableStateChanged += OnTwoShiftsEnableStateChanged;
        _twoShiftsWorkersToggle.Open(_twoShiftsWorkplace);
      }
    }

    public void ClearFragment() {
      _workplace = null;
      _workplaceDescriber = null;
      if (_twoShiftsWorkplace != null) {
        _twoShiftsWorkplace.TwoShiftsEnableStateChanged -= OnTwoShiftsEnableStateChanged;
      }
      _twoShiftsWorkplace = null;
      _workerTypeToggle.Clear();
      _priorityToggleGroup.Disable();
      _root.ToggleDisplayStyle(false);
      _increase.Unbind();
      _decrease.Unbind();
    }

    public void UpdateFragment() {
      if (_workplace != null) {
        _priorityToggleGroup.UpdateGroup();
        UpdateButtons();
        if (_workplace.Enabled) {
          UpdateViews();
        }
        _workplaceUsers.ToggleDisplayStyle(_workplace.Enabled);
        if (_botPopulation.BotCreated) {
          _workerTypeToggle.Update();
          _workerTypeToggle.Root.ToggleDisplayStyle(true);
        } else {
          _workerTypeToggle.Root.ToggleDisplayStyle(false);
        }
        _root.ToggleDisplayStyle(true);
        _twoShiftsEnabledToggle.SetEnabled(_twoShiftsWorkplace.CanBeEnabled);
        if (_twoShiftsWorkplace.TwoShiftsEnabled) {
          _twoShiftsWorkersToggle.Update();
        }
      } else {
        _root.ToggleDisplayStyle(false);
      }
    }

    private void AddEmptyViews(WorkplaceWorkerType workplaceWorkerType) {
      var workersToShow = _twoShiftsWorkplace.TwoShiftsEnabled
          ? _workplace.MaxWorkers / 2
          : _workplace.MaxWorkers;
      RemoveViews();
      for (var index = 0; index < workersToShow; ++index) {
        AddEmptyView(workplaceWorkerType);
      }
    }

    private void RemoveViews() {
      _workplaceUsers.Clear();
      _views.Clear();
    }

    private void AddEmptyView(WorkplaceWorkerType workplaceWorkerType) {
      var workerView = _workerViewFactory.Create(workplaceWorkerType);
      workerView.ShowEmpty();
      _views.Add(workerView);
      _workplaceUsers.Add(workerView.Root);
    }

    private void UpdateViews() {
      var workers = _workplace.AssignedWorkers;
      var index = 0;
      foreach (var worker in workers) {
        if (ShouldBeShown(worker) && index < _views.Count) {
          var character = worker.GetComponent<Character>();
          var view = _views[index];
          var onClick = (Action) (() => _entitySelectionService.SelectAndFollow(character));
          view.Fill(character, onClick, character.FirstName);
          ++index;
        }
      }
      var component = _workplace.GetComponent<DistrictBuilding>();
      var desiredWorkers = _twoShiftsWorkplace.TwoShiftsEnabled
          ? _workplace.DesiredWorkers / 2
          : _workplace.DesiredWorkers;
      if (_workplace.Understaffed && (bool) (BaseComponent) component.InstantDistrict) {
        if (desiredWorkers > _views.Count) {
          Debug.LogError($"{nameof(TwoShiftsWorkplaceFragment)}: Not enough views to show all "
                         + $"workers. Desired: {desiredWorkers}, Current: {_views.Count}, "
                         + $"Workplace: {component.Name}, MaxWorkers: {_workplace.MaxWorkers}, "
                         + $"TwoShiftsEnabled: {_twoShiftsWorkplace.TwoShiftsEnabled}");
        }
        for (; index < desiredWorkers; ++index) {
          _views[index].ShowVacant();
        }
      }
      for (; index < _views.Count; ++index) {
        _views[index].ShowEmpty();
      }
    }

    private void UpdateButtons() {
      var desiredWorkers = _workplace.DesiredWorkers;
      var maxWorkers = _workplace.MaxWorkers;
      var displayedDesiredWorkers = _twoShiftsWorkplace.TwoShiftsEnabled
          ? desiredWorkers / 2
          : desiredWorkers;
      _text.text = _loc.T(CurrentWorkersLocKey, GetNumberOfWorkersToShow(),
                          displayedDesiredWorkers);
      if (displayedDesiredWorkers > 1) {
        _decrease.Enable();
      } else {
        _decrease.Disable();
      }
      if (desiredWorkers < maxWorkers) {
        _increase.Enable();
      } else {
        _increase.Disable();
      }
    }

    private int GetNumberOfWorkersToShow() {
      if (!_twoShiftsWorkplace.TwoShiftsEnabled) {
        return _workplace.NumberOfAssignedWorkers;
      }
      if (_twoShiftsWorkersToggle.FirstShiftShown) {
        return _workplace.NumberOfAssignedWorkers - _workplace.NumberOfAssignedWorkers / 2;
      }
      return _workplace.NumberOfAssignedWorkers / 2;
    }

    private string GetTwoShiftsTooltip() {
      return _twoShiftsWorkplace.CanBeEnabled ? null : _loc.T(SecondShiftDisabledLocKey);
    }

    private bool ShouldBeShown(Worker worker) {
      if (_twoShiftsWorkplace.TwoShiftsEnabled) {
        return !worker.GetComponent<TwoShiftsWorkingHours>().IsSecondShiftWorker()
               == _twoShiftsWorkersToggle.FirstShiftShown;
      }
      return true;
    }

    private void OnTwoShiftsEnableStateChanged(object sender, EventArgs e) {
      _twoShiftsEnabledToggle.SetValueWithoutNotify(_twoShiftsWorkplace.TwoShiftsEnabled);
      if (_twoShiftsWorkplace.TwoShiftsEnabled) {
        _twoShiftsWorkersToggle.Show();
      } else {
        _twoShiftsWorkersToggle.Hide();
      }
    }

    private void ToggleTwoShifts(ChangeEvent<bool> evt) {
      if (evt.newValue) {
        _twoShiftsWorkplace.EnableTwoShifts();
      } else {
        _twoShiftsWorkplace.DisableTwoShifts();
      }
    }

  }
}