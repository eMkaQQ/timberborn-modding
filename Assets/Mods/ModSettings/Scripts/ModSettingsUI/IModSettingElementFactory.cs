using UnityEngine.UIElements;

namespace ModSettingsUI {
  public interface IModSettingElementFactory {

    int Priority { get; }

    bool TryCreateElement(object modSetting, VisualElement parent);

  }
}