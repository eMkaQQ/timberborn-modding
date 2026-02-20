using Timberborn.Common;
using Timberborn.Goods;
using UnityEngine;

namespace Riverborne.Core {
  public class RaftCargoRowFactory {

    private static readonly string RowNamePrefix = "#Row";
    private readonly GoodIconVisualizer _goodIconVisualizer;

    public RaftCargoRowFactory(GoodIconVisualizer goodIconVisualizer) {
      _goodIconVisualizer = goodIconVisualizer;
    }

    public RaftCargoRow CreateFirstRow(GameObject root) {
      var rowName = RowNamePrefix + "1";
      var rowObject = root.FindChild(rowName);
      var raftCargoSingle = RaftCargoSingle.Create(rowObject, rowName);
      var raftCargoBoxAndBarrel = RaftCargoBoxAndBarrel.Create(_goodIconVisualizer,
                                                               rowObject,
                                                               rowName,
                                                               tryShowBoth: true);
      return new(raftCargoSingle, raftCargoBoxAndBarrel, prioritizeSingleItem: false);
    }

    public RaftCargoRow CreateSecondRow(GameObject root) {
      var rowName = RowNamePrefix + "2";
      var rowObject = root.FindChild(rowName);
      var raftCargoSingle = RaftCargoSingle.Create(rowObject, rowName);
      var raftCargoBoxAndBarrel = RaftCargoBoxAndBarrel.Create(_goodIconVisualizer,
                                                               rowObject,
                                                               rowName,
                                                               tryShowBoth: true);
      return new(raftCargoSingle, raftCargoBoxAndBarrel, prioritizeSingleItem: true);
    }

    public RaftCargoRow CreateThirdRow(GameObject root) {
      var rowName = RowNamePrefix + "3";
      var rowObject = root.FindChild(rowName);
      var raftCargoSingle = RaftCargoSingle.Create(rowObject, rowName);
      var raftCargoBoxAndBarrel = RaftCargoBoxAndBarrel.Create(_goodIconVisualizer,
                                                               rowObject,
                                                               rowName,
                                                               tryShowBoth: false);
      return new(raftCargoSingle, raftCargoBoxAndBarrel, prioritizeSingleItem: true);
    }

  }
}