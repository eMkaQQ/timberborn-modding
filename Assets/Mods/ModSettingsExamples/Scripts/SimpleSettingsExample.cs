using ModSettings;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace ModSettingsExamples {
  internal class SimpleSettingsExample : ModSettingsOwner {

    public ModSetting<int> IntSetting { get; } = new("eMka.ModSettingsExamples.IntSetting", 2);

    public ModSetting<float> FloatSetting { get; } =
      new("eMka.ModSettingsExamples.FloatSetting", 1.1f);

    public ModSetting<string> StringSetting { get; } =
      new("eMka.ModSettingsExamples.StringSetting", "default");

    public ModSetting<bool> BoolSetting { get; } =
      new("eMka.ModSettingsExamples.BoolSetting", false);

    public SimpleSettingsExample(ISettings settings,
                                 ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                 ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.ModSettingsExamples.SimpleSettingsHeader";

    protected override string ModId => "eMka.ModSettingsExamples";

  }
}