using GoodStatistics.UI;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlItem {

    public VisualElement Root { get; }
    private readonly ResourceCountHistoryElement _resourceCountHistoryElement;

    public GoodStatisticsBatchControlItem(VisualElement root,
                                          ResourceCountHistoryElement resourceCountHistoryElement) {
      Root = root;
      _resourceCountHistoryElement = resourceCountHistoryElement;
    }

    public void Update() {
      _resourceCountHistoryElement.Update();
    }

  }
}