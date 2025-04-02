namespace ModSettings.Core {
  public class ModSettingChangedEvent {

    public ModSetting ModSetting { get; }
    
    public ModSettingChangedEvent(ModSetting modSetting) {
      ModSetting = modSetting;
    }

  }
}