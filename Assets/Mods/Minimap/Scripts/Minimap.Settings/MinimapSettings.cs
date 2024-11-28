using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;

namespace Minimap.Settings {
  public class MinimapSettings : ModSettingsOwner {

    public RangeIntModSetting ChunkSize { get; } =
      new(32, 8, 256,
          ModSettingDescriptor.CreateLocalized("eMka.Minimap.Settings.ChunkSize")
              .SetLocalizedTooltip("eMka.Minimap.Settings.ChunkSizeTooltip"));

    public ModSetting<int> DisableSize { get; } =
      new(0, ModSettingDescriptor.CreateLocalized("eMka.Minimap.Settings.DisableSize"));

    private readonly EventBus _eventBus;

    public MinimapSettings(ISettings settings,
                           ModSettingsOwnerRegistry modSettingsOwnerRegistry,
                           ModRepository modRepository,
                           EventBus eventBus) : base(
        settings, modSettingsOwnerRegistry, modRepository) {
      _eventBus = eventBus;
    }

    protected override string ModId => "eMka.Minimap";

    protected override void OnAfterLoad() {
      ChunkSize.ValueChanged += OnChunkSizeChanged;
    }

    private void OnChunkSizeChanged(object sender, int e) {
      _eventBus.Post(new MinimapSettingsChangedEvent(true));
    }

  }
}