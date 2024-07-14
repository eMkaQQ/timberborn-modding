using ModSettings.Core;

namespace ModSettings.Common {
  public class LongStringModSetting : ModSetting<string> {

    public LongStringModSetting(string locKey, string defaultValue) : base(locKey, defaultValue) {
    }

  }
}