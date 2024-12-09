namespace ModSettings.Core {
  public interface IModSettingsContextProvider {

    ModSettingsContext Context { get; }

    string WarningLocKey { get; }

  }
}