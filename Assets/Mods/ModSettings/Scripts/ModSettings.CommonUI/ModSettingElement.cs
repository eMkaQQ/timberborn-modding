using ModSettings.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  public class ModSettingElement : IModSettingElement {

    public VisualElement Root { get; }

    public ModSettingElement(VisualElement root) {
      Root = root;
    }

    public bool ShouldBlockInput => false;

  }
}