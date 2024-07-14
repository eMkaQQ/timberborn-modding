using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;

namespace ModSettingsExamples {
  public class FileStoredSettingsExample : ModSettingsOwner {

    public ModSetting<string> LongStringSetting { get; } =
      new LongStringModSetting("eMka.ModSettingsExamples.LongStringSetting",
                               "Type your long string here...");

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