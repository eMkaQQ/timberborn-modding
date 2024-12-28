using ModSettings.Core;

namespace Minimap.CoreUI {
  internal class MinimapRotationSetting : NonPersistentSetting {

    private readonly MinimapElementRotator _minimapElementRotator;

    public MinimapRotationSetting(ModSettingDescriptor descriptor,
                                  MinimapElementRotator minimapElementRotator) : base(descriptor) {
      _minimapElementRotator = minimapElementRotator;
    }

    public void RotateClockwise() {
      _minimapElementRotator.RotateClockwise();
    }

    public void RotateCounterClockwise() {
      _minimapElementRotator.RotateCounterClockwise();
    }

    public override void Reset() {
      _minimapElementRotator.Reset();
    }

  }
}