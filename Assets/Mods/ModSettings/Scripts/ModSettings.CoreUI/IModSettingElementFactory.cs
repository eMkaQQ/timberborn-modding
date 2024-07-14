using ModSettings.Core;

namespace ModSettings.CoreUI {
  public interface IModSettingElementFactory {

    int Priority { get; }

    bool TryCreateElement(ModSetting modSetting, out IModSettingElement element);

  }
}