using ModSettings.Core;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  public class ModSettingDescriptorInitializer {

    private readonly ILoc _loc;
    private readonly ITooltipRegistrar _tooltipRegistrar;

    public ModSettingDescriptorInitializer(ILoc loc,
                                           ITooltipRegistrar tooltipRegistrar) {
      _loc = loc;
      _tooltipRegistrar = tooltipRegistrar;
    }

    public void Initialize(VisualElement descriptor, ModSetting modSetting) {
      descriptor.Q<Label>("SettingLabel").text =
          modSetting.Descriptor.Name ?? _loc.T(modSetting.Descriptor.NameLocKey);
      var tooltip = GetTooltip(modSetting.Descriptor);
      var tooltipElement = descriptor.Q<VisualElement>("SettingTooltip");
      if (tooltip != null) {
        _tooltipRegistrar.Register(tooltipElement, tooltip);
      } else {
        tooltipElement.ToggleDisplayStyle(false);
      }
    }

    private string GetTooltip(ModSettingDescriptor modSettingDescriptor) {
      if (!string.IsNullOrEmpty(modSettingDescriptor.Tooltip)) {
        return modSettingDescriptor.Tooltip;
      }
      if (!string.IsNullOrEmpty(modSettingDescriptor.TooltipLocKey)) {
        return _loc.T(modSettingDescriptor.TooltipLocKey);
      }
      return null;
    }

  }
}