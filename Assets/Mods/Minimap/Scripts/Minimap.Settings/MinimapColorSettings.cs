using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;

namespace Minimap.Settings {
  public class MinimapColorSettings : ModSettingsOwner {

    public ColorModSetting LowestTerrainColor { get; } =
      new("60563C", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.LowestTerrainColor"), false);

    public ColorModSetting HighestTerrainColor { get; } =
      new("A58C67", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.HighestTerrainColor"), false);

    public ColorModSetting LowestGrassColor { get; } =
      new("589023", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.LowestGrassColor"), false);

    public ColorModSetting HighestGrassColor { get; } =
      new("8ADA3E", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.HighestGrassColor"), false);

    public ColorModSetting ShallowWaterColor { get; } =
      new("336BD3", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.ShallowWaterColor"), false);

    public ColorModSetting DeepWaterColor { get; } =
      new("1B438C", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.DeepWaterColor"), false);

    public ColorModSetting ShallowBadwaterColor { get; } =
      new("902C17", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.ShallowBadwaterColor"), false);

    public ColorModSetting DeepBadwaterColor { get; } =
      new("4E2E2A", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.DeepBadwaterColor"), false);

    public ColorModSetting TreeColor { get; } =
      new("37592A", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.TreeColor"), false);

    public ColorModSetting DeadTreeColor { get; } =
      new("524633", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.DeadTreeColor"), false);

    public ColorModSetting PlantColor { get; } =
      new("9C4D68", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.PlantColor"), false);

    public ColorModSetting WaterPlantColor { get; } =
      new("5D4998", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.WaterPlantColor"), false);

    public ColorModSetting DeadPlantColor { get; } =
      new("645167", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.DeadPlantColor"), false);

    public ColorModSetting PathColor { get; } =
      new("C9C6B6", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.PathColor"), false);

    public ColorModSetting BuildingColor { get; } =
      new("E1882B", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.BuildingColor"), false);

    public ColorModSetting BlockObjectColor { get; } =
      new("808080", ModSettingDescriptor.CreateLocalized(
              "eMka.Minimap.Settings.BlockObjectColor"), false);

    private readonly EventBus _eventBus;

    public MinimapColorSettings(ISettings settings,
                                ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                                ModRepository modRepository,
                                EventBus eventBus) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
      _eventBus = eventBus;
    }

    public override int Order => 1;

    public override string HeaderLocKey => "eMka.Minimap.Settings.Colors";

    protected override string ModId => "eMka.Minimap";

    protected override void OnAfterLoad() {
      LowestTerrainColor.ValueChanged += NotifySettingsChanged;
      HighestTerrainColor.ValueChanged += NotifySettingsChanged;
      LowestGrassColor.ValueChanged += NotifySettingsChanged;
      HighestGrassColor.ValueChanged += NotifySettingsChanged;
      ShallowWaterColor.ValueChanged += NotifySettingsChanged;
      DeepWaterColor.ValueChanged += NotifySettingsChanged;
      ShallowBadwaterColor.ValueChanged += NotifySettingsChanged;
      DeepBadwaterColor.ValueChanged += NotifySettingsChanged;
      TreeColor.ValueChanged += NotifySettingsChanged;
      DeadTreeColor.ValueChanged += NotifySettingsChanged;
      PlantColor.ValueChanged += NotifySettingsChanged;
      WaterPlantColor.ValueChanged += NotifySettingsChanged;
      DeadPlantColor.ValueChanged += NotifySettingsChanged;
      PathColor.ValueChanged += NotifySettingsChanged;
      BuildingColor.ValueChanged += NotifySettingsChanged;
      BlockObjectColor.ValueChanged += NotifySettingsChanged;
    }

    private void NotifySettingsChanged(object sender, string s) {
      _eventBus.Post(new MinimapSettingsChangedEvent(false));
    }

  }
}