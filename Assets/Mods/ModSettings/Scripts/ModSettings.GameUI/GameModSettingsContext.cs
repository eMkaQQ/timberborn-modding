using ModSettings.Core;

namespace ModSettings.GameUI {
  internal class GameModSettingsContext : IModSettingsContextProvider {

    public ModSettingsContext Context => ModSettingsContext.Game;
    public string WarningLocKey => "ModSettingsBox.GameNotChangeable";

  }
}