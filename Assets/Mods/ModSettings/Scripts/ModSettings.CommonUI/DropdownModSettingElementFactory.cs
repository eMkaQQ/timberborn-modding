using ModSettings.Common;
using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  internal class DropdownModSettingElementFactory : IModSettingElementFactory {

    private readonly VisualElementLoader _visualElementLoader;
    private readonly ModSettingDescriptorInitializer _modSettingDescriptorInitializer;
    private readonly DropdownItemsSetter _dropdownItemsSetter;

    public DropdownModSettingElementFactory(VisualElementLoader visualElementLoader,
                                            ModSettingDescriptorInitializer
                                                modSettingDescriptorInitializer,
                                            DropdownItemsSetter dropdownItemsSetter) {
      _visualElementLoader = visualElementLoader;
      _modSettingDescriptorInitializer = modSettingDescriptorInitializer;
      _dropdownItemsSetter = dropdownItemsSetter;
    }

    public int Priority => 100;

    public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element) {
      if (modSetting is LimitedStringModSetting limitedString) {
        var root = _visualElementLoader.LoadVisualElement("ModSettings/DropdownModSettingElement");
        _modSettingDescriptorInitializer.Initialize(root.Q<VisualElement>("Descriptor"),
                                                    limitedString);
        var dropdown = root.Q<Dropdown>("Dropdown");
        if (limitedString.IsLocalized) {
          _dropdownItemsSetter.SetLocalizableItems(
              dropdown, LimitedStringDropdownProvider.Create(limitedString));
        } else {
          _dropdownItemsSetter.SetItems(
              dropdown, NonLocalizedLimitedStringDropdownProvider.Create(limitedString));
        }
        element = new ModSettingElement(root);
        return true;
      }
      element = null;
      return false;
    }

  }
}