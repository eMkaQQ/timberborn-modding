using Riverborne.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.InventorySystemUI;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  public class RaftFragment : IEntityPanelFragment {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly InventoryFragmentBuilderFactory _inventoryFragmentBuilderFactory;
    private InventoryFragment _inventoryFragment;
    private Raft _raft;
    private VisualElement _root;

    public RaftFragment(VisualElementLoader visualElementLoader,
                        InventoryFragmentBuilderFactory inventoryFragmentBuilderFactory) {
      _visualElementLoader = visualElementLoader;
      _inventoryFragmentBuilderFactory = inventoryFragmentBuilderFactory;
    }

    public VisualElement InitializeFragment() {
      _root = _visualElementLoader.LoadVisualElement("Game/EntityPanel/GoodStackFragment");
      _inventoryFragment = _inventoryFragmentBuilderFactory.CreateBuilder(_root).Build();
      return _root;
    }

    public void ShowFragment(BaseComponent entity) {
      _raft = entity.GetComponent<Raft>();
      if (_raft) {
        _inventoryFragment.ShowFragment(_raft.Inventory);
      }
    }

    public void ClearFragment() {
      _raft = null;
      _inventoryFragment.ClearFragment();
    }

    public void UpdateFragment() {
      if (_raft && _raft.Enabled) {
        _inventoryFragment.UpdateFragment();
        _root.ToggleDisplayStyle(true);
      } else {
        _root.ToggleDisplayStyle(false);
      }
    }

  }
}