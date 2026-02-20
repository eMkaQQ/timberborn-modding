using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace TimberPhysics.WaterSettings {
  public class FloatingObjectSettingOwner : ModSettingsOwner {

    public ModSetting<bool> FloatingObjectsCollisionSetting { get; } =
      new(true,
          ModSettingDescriptor.CreateLocalized("TimberPhysics.FloatingObjectsCollisionSetting"));

    public FloatingObjectSettingOwner(ISettings settings,
                                      ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                      ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    protected override string ModId => "eMka.TimberPhysics";

  }
}