using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;

namespace ModSettingsExamples {
  public class FileStoredSettingsExample : ModSettingsOwner {

    private static readonly string NameKey = "eMka.ModSettingsExamples.LongStringSetting";
    private static readonly string TooltipKey = "eMka.ModSettingsExamples.LongStringTooltip";

    public ModSetting<string> LongStringSetting { get; } =
      new LongStringModSetting("Type your long string here...",
                               ModSettingDescriptor.CreateLocalized(NameKey)
                                   .SetLocalizedTooltip(TooltipKey));

    public FileStoredSettingsExample(DefaultModFileStoredSettings defaultModFileStoredSettings,
                                     ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                     ModRepository modRepository) : base(
        defaultModFileStoredSettings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.ModSettingsExamples.FileStoredSettingsHeader";

    public override int Order => 20;

    protected override string ModId => "eMka.ModSettingsExamples";

  }
}