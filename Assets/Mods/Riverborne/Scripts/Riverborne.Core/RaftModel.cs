using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;

namespace Riverborne.Core {
  internal class RaftModel : BaseComponent,
                             IAwakableComponent,
                             IPostInitializableEntity {

    private readonly IGoodService _goodService;
    private readonly RaftCargoRowFactory _raftCargoRowFactory;
    private Raft _raft;
    private RaftCargoRow _row1;
    private RaftCargoRow _row2;
    private RaftCargoRow _row3;
    private readonly RaftCargo _cargo = new();

    public RaftModel(IGoodService goodService,
                     RaftCargoRowFactory raftCargoRowFactory) {
      _goodService = goodService;
      _raftCargoRowFactory = raftCargoRowFactory;
    }

    public void Awake() {
      _raft = GetComponent<Raft>();
    }

    public void PostInitializeEntity() {
      _raft.Inventory.InventoryStockChanged += OnInventoryStockChanged;
      _row1 = _raftCargoRowFactory.CreateFirstRow(_raft.GameObject);
      _row2 = _raftCargoRowFactory.CreateSecondRow(_raft.GameObject);
      _row3 = _raftCargoRowFactory.CreateThirdRow(_raft.GameObject);
      UpdateModel();
    }

    private void OnInventoryStockChanged(object sender, InventoryAmountChangedEventArgs e) {
      UpdateModel();
    }

    private void UpdateModel() {
      _cargo.UpdateItems(GetCurrentCargoItems());

      if (ShouldShowEmpty()) {
        ShowEmpty();
      } else if (ShouldShowSingleRow()) {
        ShowSingleRow();
      } else if (ShouldShowThreeRows()) {
        ShowThreeRows();
      } else {
        ShowTwoRows();
      }
    }

    private IEnumerable<RaftCargoItem> GetCurrentCargoItems() {
      foreach (var item in _raft.Inventory.Stock) {
        var amount = item.GoodId == Raft.MaterialGood
            ? item.Amount - Raft.MaterialAmount
            : item.Amount;
        if (amount > 0) {
          yield return new(_goodService.GetGood(item.GoodId), amount);
        }
      }
    }

    private bool ShouldShowEmpty() {
      return _cargo.Items.Count == 0;
    }

    private void ShowEmpty() {
      _row1.Hide();
      _row2.Hide();
      _row3.Hide();
    }

    private bool ShouldShowSingleRow() {
      if (_cargo.Items.Count == 1) {
        return true;
      }
      if (_cargo.Items.Count == 2
          && _cargo.Items[0].IsBoxOrBarrel()
          && _cargo.Items[1].IsBoxOrBarrel()) {
        return true;
      }
      return false;
    }

    private void ShowSingleRow() {
      _row1.Hide();
      _row2.Update(_cargo);
      _row3.Hide();
    }

    private bool ShouldShowThreeRows() {
      foreach (var raftCargoItem in _cargo.Items) {
        if (raftCargoItem.IsBoxOrBarrel() || raftCargoItem.IsBag()) {
          return false;
        }
      }
      return true;
    }

    private void ShowThreeRows() {
      _row1.Update(_cargo);
      _row2.Update(_cargo);
      _row3.Update(_cargo);
    }

    private void ShowTwoRows() {
      _row1.Update(_cargo);
      _row2.Hide();
      _row3.Update(_cargo);
    }

  }
}