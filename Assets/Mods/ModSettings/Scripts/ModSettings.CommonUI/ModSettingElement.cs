using ModSettings.Core;
using ModSettings.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  public class ModSettingElement : IModSettingElement {

    public VisualElement Root { get; }
    public ModSetting ModSetting { get; }

    public ModSettingElement(VisualElement root, 
                             ModSetting modSetting) {
      Root = root;
      ModSetting = modSetting;
    }

    public bool ShouldBlockInput => false;

  }
}