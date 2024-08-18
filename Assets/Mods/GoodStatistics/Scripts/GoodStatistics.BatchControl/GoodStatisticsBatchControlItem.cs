using GoodStatistics.UI;
using UnityEngine.UIElements;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlItem {

    public VisualElement Root { get; }
    private readonly GoodSampleRecordsElement _goodSampleRecordsElement;

    public GoodStatisticsBatchControlItem(VisualElement root,
                                          GoodSampleRecordsElement goodSampleRecordsElement) {
      Root = root;
      _goodSampleRecordsElement = goodSampleRecordsElement;
    }

    public void Update() {
      _goodSampleRecordsElement.Update();
    }

  }
}