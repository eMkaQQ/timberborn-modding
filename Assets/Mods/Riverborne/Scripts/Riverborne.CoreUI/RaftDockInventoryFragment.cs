using Riverborne.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  internal class RaftDockInventoryFragment : IEntityPanelFragment {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly RaftDockInventoryRowUpdater _raftDockInventoryRowUpdater;
    private RaftDockInventory _raftDockInventory;
    private VisualElement _root;
    private ScrollView _inventoryContent;
    private VisualElement _isEmpty;

    public RaftDockInventoryFragment(VisualElementLoader visualElementLoader,
                                     RaftDockInventoryRowUpdater raftDockInventoryRowUpdater) {
      _visualElementLoader = visualElementLoader;
      _raftDockInventoryRowUpdater = raftDockInventoryRowUpdater;
    }

    public VisualElement InitializeFragment() {
      var elementName = "Game/EntityPanel/WorkplaceInventoryFragment";
      _root = _visualElementLoader.LoadVisualElement(elementName);
      _inventoryContent = _root.Q<ScrollView>("Content");
      _isEmpty = _root.Q<VisualElement>("IsEmpty");
      _root.ToggleDisplayStyle(false);
      return _root;
    }

    public void ShowFragment(BaseComponent entity) {
      _raftDockInventory = entity.GetComponent<RaftDockInventory>();
      if (_raftDockInventory) {
        _root.ToggleDisplayStyle(true);
        _raftDockInventoryRowUpdater.AddRows(_inventoryContent, _raftDockInventory.Inventory);
      }
    }

    public void ClearFragment() {
      if (_raftDockInventory) {
        _raftDockInventory = null;
        _inventoryContent.Clear();
        _raftDockInventoryRowUpdater.ClearRows();
        _root.ToggleDisplayStyle(false);
      }
    }

    public void UpdateFragment() {
      if (_raftDockInventory) {
        _raftDockInventoryRowUpdater.UpdateRowsVisibility(_root, _isEmpty,
                                                          _raftDockInventory.Inventory);
      }
    }

  }
}