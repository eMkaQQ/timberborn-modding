using Timberborn.Common;
using Timberborn.Goods;
using UnityEngine;

namespace Riverborne.Core {
  public class RaftCargoSingle {

    private static readonly string ScrapName = ".Scrap";
    private static readonly string LogName = ".Log";
    private static readonly string PlankName = ".Plank";
    private static readonly string TreatedPlankName = ".TreatedPlank";
    private static readonly string MetalBlockName = ".MetalBlock";
    private static readonly string BagName = ".Bag";

    private readonly GameObject _scrap;
    private readonly GameObject _log;
    private readonly GameObject _plank;
    private readonly GameObject _treatedPlank;
    private readonly GameObject _metalBlock;
    private readonly GameObject _bag;

    private RaftCargoSingle(GameObject scrap,
                            GameObject log,
                            GameObject plank,
                            GameObject treatedPlank,
                            GameObject metalBlock,
                            GameObject bag) {
      _scrap = scrap;
      _log = log;
      _plank = plank;
      _treatedPlank = treatedPlank;
      _metalBlock = metalBlock;
      _bag = bag;
    }

    public static RaftCargoSingle Create(GameObject root, string rowName) {
      return new(root.FindChild(rowName + ScrapName),
                 root.FindChild(rowName + LogName),
                 root.FindChild(rowName + PlankName),
                 root.FindChild(rowName + TreatedPlankName),
                 root.FindChild(rowName + MetalBlockName),
                 root.FindChild(rowName + BagName));
    }

    public bool TryShow(RaftCargo cargo) {
      foreach (var item in cargo.Items) {
        if (TryShow(item)) {
          cargo.MoveToLast(item);
          return true;
        }
      }
      return false;
    }

    public void Hide() {
      _scrap.SetActive(false);
      _log.SetActive(false);
      _plank.SetActive(false);
      _treatedPlank.SetActive(false);
      _metalBlock.SetActive(false);
      _bag.SetActive(false);
    }

    private bool TryShow(RaftCargoItem cargoItem) {
      switch (cargoItem.GoodSpec.VisibleContainer) {
        case VisibleContainer.Scrap:
          _scrap.SetActive(true);
          return true;
        case VisibleContainer.Log:
          _log.SetActive(true);
          return true;
        case VisibleContainer.Plank:
          _plank.SetActive(true);
          return true;
        case VisibleContainer.TreatedPlank:
          _treatedPlank.SetActive(true);
          return true;
        case VisibleContainer.MetalBlock:
          _metalBlock.SetActive(true);
          return true;
        case VisibleContainer.Bag:
          _bag.SetActive(true);
          return true;
        case VisibleContainer.Box:
        case VisibleContainer.Barrel:
          return false;
        default:
          throw new System.ArgumentOutOfRangeException(nameof(cargoItem.GoodSpec.VisibleContainer),
                                                       cargoItem.GoodSpec.VisibleContainer, null);
      }
    }

  }
}