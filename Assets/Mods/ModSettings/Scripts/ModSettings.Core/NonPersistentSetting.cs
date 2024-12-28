namespace ModSettings.Core {
  public abstract class NonPersistentSetting : ModSetting {

    protected NonPersistentSetting(ModSettingDescriptor descriptor) : base(descriptor) {
    }

    public override void Reset() {
    }

  }
}