using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace Minimap.CoreUI {
  internal class MinimapRotationSettingOwner : ModSettingsOwner {

    private readonly MinimapElementRotator _minimapElementRotator;

    public MinimapRotationSettingOwner(ISettings settings,
                                       ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                       ModRepository modRepository,
                                       MinimapElementRotator minimapElementRotator) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
      _minimapElementRotator = minimapElementRotator;
    }

    public override int Order => -10;

    public override string HeaderLocKey => "eMka.Minimap.Settings.MinimapRotationHeader";

    public override ModSettingsContext ChangeableOn =>
        ModSettingsContext.Game | ModSettingsContext.MapEditor;

    protected override string ModId => "eMka.Minimap";

    protected override void OnAfterLoad() {
      var rotationSetting =
          new MinimapRotationSetting(
              ModSettingDescriptor.CreateLocalized("eMka.Minimap.Settings.MinimapRotation"),
              _minimapElementRotator);
      AddNonPersistentModSetting(rotationSetting);
    }

  }
}