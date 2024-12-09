using ModSettings.Core;

namespace ModSettings.MainMenuUI {
  internal class MainMenuModSettingsContext : IModSettingsContextProvider {

    public ModSettingsContext Context => ModSettingsContext.MainMenu;
    public string WarningLocKey => "ModSettingsBox.MainMenuNotChangeable";

  }
}