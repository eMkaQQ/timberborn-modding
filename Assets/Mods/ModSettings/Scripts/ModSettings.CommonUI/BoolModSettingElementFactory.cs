using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class BoolModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public BoolModSettingElementFactory(VisualElementLoader visualElementLoader,
                                        ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public int Priority => 0;

    public bool TryCreateElement(object modSetting, VisualElement parent) {
      if (modSetting is ModSetting<bool> boolModSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/BoolModSettingElement");
        root.Q<Label>("SettingLabel").text = _loc.T(boolModSetting.LocKey);
        var toggle = root.Q<Toggle>();
        toggle.value = boolModSetting.Value;
        toggle.RegisterValueChangedCallback(evt => boolModSetting.SetValue(evt.newValue));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}