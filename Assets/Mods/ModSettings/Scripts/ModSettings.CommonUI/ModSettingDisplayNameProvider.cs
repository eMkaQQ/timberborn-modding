using ModSettings.Core;
using Timberborn.Localization;

namespace ModSettings.CommonUI {
  internal class ModSettingDisplayNameProvider {

    private readonly ILoc _loc;

    public ModSettingDisplayNameProvider(ILoc loc) {
      _loc = loc;
    }

    public string Get(ModSetting modSetting) {
      return modSetting.DisplayName ?? _loc.T(modSetting.LocKey);
    }

  }
}