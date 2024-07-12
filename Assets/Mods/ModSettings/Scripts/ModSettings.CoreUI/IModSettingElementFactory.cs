using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  public interface IModSettingElementFactory {

    int Priority { get; }

    bool TryCreateElement(object modSetting, VisualElement parent);

  }
}