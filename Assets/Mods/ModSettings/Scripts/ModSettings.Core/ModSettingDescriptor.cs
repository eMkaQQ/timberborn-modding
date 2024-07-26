namespace ModSettings.Core {
  public class ModSettingDescriptor {

    public string NameLocKey { get; }
    public string Name { get; }
    public string TooltipLocKey { get; private set; }
    public string Tooltip { get; private set; }

    private ModSettingDescriptor(string nameLocKey,
                                 string name) {
      NameLocKey = nameLocKey;
      Name = name;
    }

    public static ModSettingDescriptor CreateLocalized(string nameLocKey) {
      return new(nameLocKey, null);
    }

    public static ModSettingDescriptor Create(string name) {
      return new(null, name);
    }

    public ModSettingDescriptor SetLocalizedTooltip(string tooltipLocKey) {
      TooltipLocKey = tooltipLocKey;
      return this;
    }

    public ModSettingDescriptor SetTooltip(string tooltip) {
      Tooltip = tooltip;
      return this;
    }

  }
}