using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.InventorySystem;
using Timberborn.InventorySystemUI;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  public class RaftDockInventoryRowUpdater {

    private readonly InformationalRowsFactory _informationalRowsFactory;
    private readonly List<RaftDockInventoryRow> _raftDockInventoryRows = new();

    public RaftDockInventoryRowUpdater(InformationalRowsFactory informationalRowsFactory) {
      _informationalRowsFactory = informationalRowsFactory;
    }

    public void AddRows(ScrollView inventoryContent, Inventory inventory) {
      foreach (var good in inventory.AllowedGoods) {
        var storableGood = good.StorableGood;
        var row = _informationalRowsFactory.CreateInputRowWithLimit(storableGood, inventory,
                                                                    inventoryContent);
        _raftDockInventoryRows.Add(new(row, true));
      }
      foreach (var good in inventory.AllowedGoods) {
        var storableGood = good.StorableGood;
        var row = _informationalRowsFactory.CreateOutputRowWithLimit(storableGood, inventory,
                                                                     inventoryContent);

        _raftDockInventoryRows.Add(new(row, false));
      }
    }

    public void UpdateRowsVisibility(VisualElement root, VisualElement isEmpty,
                                     Inventory inventory) {
      if (inventory && inventory.Enabled) {
        var anyShown = false;
        root.ToggleDisplayStyle(true);
        foreach (var row in _raftDockInventoryRows) {
          if (ShouldBeShown(row, inventory)) {
            row.InformationalRow.ShowUpdated();
            anyShown = true;
          } else {
            row.InformationalRow.Hide();
          }
        }
        isEmpty.ToggleDisplayStyle(!anyShown);
      } else {
        root.ToggleDisplayStyle(false);
      }
    }
    
    public void ClearRows() {
      _raftDockInventoryRows.Clear();
    }

    private static bool ShouldBeShown(RaftDockInventoryRow row, Inventory inventory) {
      if (inventory.AmountInStock(row.InformationalRow.GoodId) == 0) {
        return false;
      }
      var isInputGood = inventory._allowedGoods._inputGoods.Contains(row.InformationalRow.GoodId);
      return row.IsInput ? isInputGood : !isInputGood;
    }

    private class RaftDockInventoryRow {

      public InformationalRow InformationalRow { get; }
      public bool IsInput { get; }

      public RaftDockInventoryRow(InformationalRow informationalRow, bool
                                      isInput) {
        InformationalRow = informationalRow;
        IsInput = isInput;
      }

    }

  }
}