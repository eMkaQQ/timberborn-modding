using System;

namespace Riverborne.Core {
  public class RaftCargoRow {

    private readonly RaftCargoSingle _raftCargoSingle;
    private readonly RaftCargoBoxAndBarrel _raftCargoBoxAndBarrel;
    private readonly bool _prioritizeSingleItem;

    public RaftCargoRow(RaftCargoSingle raftCargoSingle,
                        RaftCargoBoxAndBarrel raftCargoBoxAndBarrel,
                        bool prioritizeSingleItem) {
      _raftCargoSingle = raftCargoSingle;
      _raftCargoBoxAndBarrel = raftCargoBoxAndBarrel;
      _prioritizeSingleItem = prioritizeSingleItem;
    }

    public void Update(RaftCargo cargo) {
      Hide();

      if (_prioritizeSingleItem) {
        if (_raftCargoSingle.TryShow(cargo)) {
          return;
        }
        if (_raftCargoBoxAndBarrel.TryShow(cargo)) {
          return;
        }
      } else {
        if (_raftCargoBoxAndBarrel.TryShow(cargo)) {
          return;
        }
        if (_raftCargoSingle.TryShow(cargo)) {
          return;
        }
      }
      throw new InvalidOperationException("Unable to show any cargo item in the row: " + cargo);
    }

    public void Hide() {
      _raftCargoSingle.Hide();
      _raftCargoBoxAndBarrel.Hide();
    }

  }
}