using Timberborn.Localization;

namespace GoodStatistics.AnalyticsUI {
  public class GoodTrendDaysLeftDescriber {

    private static readonly string DaysLocKey = "Time.DaysShort";
    private static readonly string NoDaysLeftText = "—";
    private readonly ILoc _loc;

    public GoodTrendDaysLeftDescriber(ILoc loc) {
      _loc = loc;
    }

    public string GetDaysLeftText(float daysLeft) {
      if (daysLeft <= 0) {
        return NoDaysLeftText;
      }
      if (daysLeft < 1) {
        return $"<{_loc.T(DaysLocKey, "1")}";
      }
      if (daysLeft > 99) {
        return $">{_loc.T(DaysLocKey, "99")}";
      }
      return _loc.T(DaysLocKey, daysLeft.ToString("F0"));
    }

  }
}