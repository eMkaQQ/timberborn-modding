using ModSettings.CommonUI;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;

namespace ModSettingsExamples {
  internal class DummyButtonElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;

    public DummyButtonElementFactory(VisualElementLoader visualElementLoader) {
      _visualElementLoader = visualElementLoader;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is DummyButton dummyButton) {
        var root = _visualElementLoader.LoadVisualElement("DummyButtonElement");
        element = new ModSettingElement(root, dummyButton);
        return true;
      }

      element = null;
      return false;
    }

  }
}