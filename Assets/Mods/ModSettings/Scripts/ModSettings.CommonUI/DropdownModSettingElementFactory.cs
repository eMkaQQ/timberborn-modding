using ModSettings.Common;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class DropdownModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;
    private readonly DropdownItemsSetter _dropdownItemsSetter;

    public DropdownModSettingElementFactory(VisualElementLoader visualElementLoader,
                                            ILoc loc,
                                            DropdownItemsSetter dropdownItemsSetter) {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
      _dropdownItemsSetter = dropdownItemsSetter;
    }

    public int Priority => 100;

    public bool TryCreateElement(object modSetting, VisualElement parent) {
      if (modSetting is LimitedStringModSetting limitedString) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/DropdownModSettingElement");
        root.Q<Label>("SettingLabel").text = _loc.T(limitedString.LocKey);
        var dropdown = root.Q<Dropdown>("Dropdown");
        _dropdownItemsSetter.SetLocalizableItems(
            dropdown, LimitedStringDropdownProvider.Create(limitedString));
        parent.Add(root);
        return true;
      }
      return false;
    }

  }
}