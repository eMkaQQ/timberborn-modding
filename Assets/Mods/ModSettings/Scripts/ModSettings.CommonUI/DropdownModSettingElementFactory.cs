using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class DropdownModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDisplayNameProvider _modSettingDisplayNameProvider;
    private readonly DropdownItemsSetter _dropdownItemsSetter;

    public DropdownModSettingElementFactory(VisualElementLoader visualElementLoader,
                                            ModSettingDisplayNameProvider
                                                modSettingDisplayNameProvider,
                                            DropdownItemsSetter dropdownItemsSetter) {
      _visualElementLoader = visualElementLoader;
      _modSettingDisplayNameProvider = modSettingDisplayNameProvider;
      _dropdownItemsSetter = dropdownItemsSetter;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is LimitedStringModSetting limitedString) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/DropdownModSettingElement");
        root.Q<Label>("SettingLabel").text = _modSettingDisplayNameProvider.Get(limitedString);
        var dropdown = root.Q<Dropdown>("Dropdown");
        _dropdownItemsSetter.SetLocalizableItems(
            dropdown, LimitedStringDropdownProvider.Create(limitedString));
        element = new ModSettingElement(root);
        return true;
      }
      element = null;
      return false;
    }

  }
}