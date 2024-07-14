using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  public interface IModSettingElement {

    VisualElement Root { get; }

    bool ShouldBlockInput { get; }

  }
}