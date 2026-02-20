using Riverborne.Core;
using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.TimeSystem;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  internal class RaftDockFragment : IEntityPanelFragment {

    private static readonly string RemoveConfirmationLocKey =
        "Riverborne.RaftDock.RemoveDispatchConfirmation";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly EditDispatchPanel _editDispatchPanel;
    private readonly RaftDispatchItemFactory _raftDispatchItemFactory;
    private readonly DialogBoxShower _dialogBoxShower;
    private readonly IDayNightCycle _dayNightCycle;
    private VisualElement _root;
    private ListView _dispatchesListView;
    private RaftDock _raftDock;
    private BlockObject _blockObject;
    private readonly List<RaftDispatch> _dispatches = new();

    public RaftDockFragment(VisualElementLoader visualElementLoader,
                            EditDispatchPanel editDispatchPanel,
                            RaftDispatchItemFactory raftDispatchItemFactory,
                            DialogBoxShower dialogBoxShower,
                            IDayNightCycle dayNightCycle) {
      _visualElementLoader = visualElementLoader;
      _editDispatchPanel = editDispatchPanel;
      _raftDispatchItemFactory = raftDispatchItemFactory;
      _dialogBoxShower = dialogBoxShower;
      _dayNightCycle = dayNightCycle;
    }

    public VisualElement InitializeFragment() {
      _root = _visualElementLoader.LoadVisualElement("Riverborne/RaftDockFragment");
      _root.Q<Button>("CreateDispatchButton")
          .RegisterCallback<ClickEvent>(_ => OpenEditDispatchPanel());
      _raftDispatchItemFactory.Initialize(EditDispatch, RemoveDispatch, ToggleDispatchPause);
      _dispatchesListView = _root.Q<ListView>("DispatchesList");
      _dispatchesListView.makeItem = _raftDispatchItemFactory.Create;
      _dispatchesListView.bindItem =
          (element, index) => _raftDispatchItemFactory.Bind(element, _dispatches[index]);
      _dispatchesListView.unbindItem = (element, _) => _raftDispatchItemFactory.Unbind(element);
      _dispatchesListView.itemsSource = _dispatches;
      _dispatchesListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity) {
      if (entity.GetComponent<RaftDock>() is { } raftDock) {
        _raftDock = raftDock;
        _blockObject = entity.GetComponent<BlockObject>();
        UpdateDispatchesListView();
        _root.ToggleDisplayStyle(true);
      }
    }

    public void ClearFragment() {
      if (_raftDock != null) {
        _root.ToggleDisplayStyle(false);
        _raftDock = null;
        _blockObject = null;
        _raftDispatchItemFactory.Clear();
        _dispatches.Clear();
      }
    }

    public void UpdateFragment() {
      if (_raftDock != null && _blockObject.IsFinished) {
        _raftDispatchItemFactory.Update();
      }
    }

    private void RemoveDispatch(RaftDispatch raftDispatch) {
      _dialogBoxShower.Create()
          .SetLocalizedMessage(RemoveConfirmationLocKey)
          .SetDefaultCancelButton()
          .SetConfirmButton(() => {
            _raftDock.RemoveRaftDispatch(raftDispatch);
            UpdateDispatchesListView();
          })
          .Show();
    }

    private void EditDispatch(RaftDispatch raftDispatch) {
      _editDispatchPanel.ShowExisting(raftDispatch, updatedDispatch => {
        _raftDock.ReplaceRaftDispatch(raftDispatch, updatedDispatch);
        UpdateDispatchesListView();
      });
    }

    private void ToggleDispatchPause(RaftDispatch raftDispatch) {
      raftDispatch.TogglePaused();
      //TODO: this needs refactor
      if (!raftDispatch.IsPaused) {
        raftDispatch.UpdateLastDispatchTime(_dayNightCycle.PartialDayNumber);
      }
    }

    private void OpenEditDispatchPanel() {
      _editDispatchPanel.ShowNew(AddNewDispatch, _raftDock);
    }

    private void AddNewDispatch(RaftDispatch raftDispatch) {
      _raftDock.AddRaftDispatch(raftDispatch);
      UpdateDispatchesListView();
    }

    private void UpdateDispatchesListView() {
      _dispatches.Clear();
      _dispatches.AddRange(_raftDock.RaftDispatches);
      _dispatchesListView.Rebuild();
    }

  }
}