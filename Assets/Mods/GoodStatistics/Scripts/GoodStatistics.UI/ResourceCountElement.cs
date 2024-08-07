using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace GoodStatistics.UI {
  public class ResourceCountElement {

    private readonly VisualElement _fillRate;

    public ResourceCountElement(VisualElement fillRate) {
      _fillRate = fillRate;
    }

    public void SetFillRate(float fillRate) {
      _fillRate.SetHeightAsPercent(fillRate);
    }

  }
}