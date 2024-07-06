using ModSettings;
using ModSettingsUI;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ModSettingsFactoriesUI {
  internal class IntModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public IntModSettingElementFactory(VisualElementLoader visualElementLoader,
                                       ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public int Priority => 0;

    public bool TryCreateElement(object modSetting, VisualElement parent) {
      if (modSetting is ModSetting<int> intSetting) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/IntModSettingElement");
        root.Q<Label>("SettingLabel").text = _loc.T(intSetting.LocKey);
        var intField = root.Q<IntegerField>();
        intField.value = intSetting.Value;
        intField.RegisterValueChangedCallback(evt => intSetting.SetValue(evt.newValue));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}