using GoodStatistics.GoodSampling;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace GoodStatistics.GoodUI {
  public class GoodSampleElement {

    public GoodSample GoodSample { get; private set; }

    private readonly VisualElement _fillRate;

    public GoodSampleElement(VisualElement fillRate) {
      _fillRate = fillRate;
    }

    public void Update(GoodSample goodSample, float fillRate) {
      GoodSample = goodSample;
      _fillRate.SetHeightAsPercent(fillRate);
    }

  }
}