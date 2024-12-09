using System;

namespace ModSettings.Core {
  [Flags]
  public enum ModSettingsContext {

    None = 0,
    MainMenu = 1,
    Game = 2,
    MapEditor = 4,
    All = MainMenu | Game | MapEditor

  }
}