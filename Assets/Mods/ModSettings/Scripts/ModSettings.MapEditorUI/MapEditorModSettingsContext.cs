using ModSettings.Core;

namespace ModSettings.MapEditorUI {
  internal class MapEditorModSettingsContext : IModSettingsContextProvider {

    public ModSettingsContext Context => ModSettingsContext.MapEditor;
    public string WarningLocKey => "ModSettingsBox.MapEditorNotChangeable";

  }
}