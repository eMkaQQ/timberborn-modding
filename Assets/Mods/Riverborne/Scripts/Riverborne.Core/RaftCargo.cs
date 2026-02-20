using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;

namespace Riverborne.Core {
  public class RaftCargo {

    private readonly List<RaftCargoItem> _items = new();
    public ReadOnlyList<RaftCargoItem> Items => _items.AsReadOnlyList();

    public void UpdateItems(IEnumerable<RaftCargoItem> items) {
      _items.Clear();
      _items.AddRange(items.OrderByDescending(item => item.Amount));
    }

    public void MoveToLast(RaftCargoItem item) {
      if (_items.Remove(item)) {
        _items.Add(item);
      }
    }

    public override string ToString() {
      return $"RaftCargo{{Items={string.Join(", ", _items.Select(it => it.GoodSpec.Id))}}}";
    }

  }
}