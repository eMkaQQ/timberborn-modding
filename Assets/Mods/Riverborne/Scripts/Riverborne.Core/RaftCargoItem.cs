using Timberborn.Goods;

namespace Riverborne.Core {
  public class RaftCargoItem {

    public GoodSpec GoodSpec { get; }
    public int Amount { get; }

    public RaftCargoItem(GoodSpec goodSpec,
                         int amount) {
      GoodSpec = goodSpec;
      Amount = amount;
    }

    public bool IsBoxOrBarrel() {
      return GoodSpec.VisibleContainer is VisibleContainer.Box or VisibleContainer.Barrel;
    }

    public bool IsBag() {
      return GoodSpec.VisibleContainer == VisibleContainer.Bag;
    }

  }
}