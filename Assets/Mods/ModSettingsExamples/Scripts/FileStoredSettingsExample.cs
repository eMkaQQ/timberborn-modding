using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;

namespace ModSettingsExamples {
  public class FileStoredSettingsExample : ModSettingsOwner {

    private static readonly string NameKey = "eMka.ModSettingsExamples.LongStringSetting";
    private static readonly string TooltipKey = "eMka.ModSettingsExamples.LongStringTooltip";

    public LongStringModSetting LongStringSetting { get; } =
      new("Type your long string here...",
          ModSettingDescriptor.CreateLocalized(NameKey).SetLocalizedTooltip(TooltipKey));

    public FileStoredSettingsExample(DefaultModFileStoredSettings defaultModFileStoredSettings,
                                     ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                     ModRepository modRepository) : base(
        defaultModFileStoredSettings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.ModSettingsExamples.FileStoredSettingsHeader";

    public override int Order => 20;

    public override ModSettingsContext ChangeableOn =>
        ModSettingsContext.MainMenu | ModSettingsContext.Game;

    protected override string ModId => "eMka.ModSettingsExamples";

  }
}