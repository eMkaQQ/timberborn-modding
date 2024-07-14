using ModSettings.Core;
using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  public interface IModSettingElementFactory {

    int Priority { get; }

    bool TryCreateElement(ModSetting modSetting, VisualElement parent);

  }
}