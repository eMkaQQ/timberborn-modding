using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace ModSettingsExamples {
  internal class SimpleSettingsExample : ModSettingsOwner {

    public ModSetting<int> IntSetting { get; } =
      new(2, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.IntSetting"));

    public ModSetting<float> FloatSetting { get; } =
      new(1.1f, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.FloatSetting"));

    public ModSetting<string> StringSetting { get; } =
      new("default",
          ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.StringSetting"));

    public ModSetting<bool> BoolSetting { get; } =
      new(false, ModSettingDescriptor.CreateLocalized("eMka.ModSettingsExamples.BoolSetting"));

    public SimpleSettingsExample(ISettings settings,
                                 ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                 ModRepository modRepository) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
    }

    public override string HeaderLocKey => "eMka.ModSettingsExamples.SimpleSettingsHeader";
    
    public override ModSettingsContext ChangeableOn => ModSettingsContext.All;

    protected override string ModId => "eMka.ModSettingsExamples";

  }
}