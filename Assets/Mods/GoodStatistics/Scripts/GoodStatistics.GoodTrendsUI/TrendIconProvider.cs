using GoodStatistics.Analytics;
using System;
using Timberborn.AssetSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace GoodStatistics.GoodTrendsUI {
  public class TrendIconProvider : ILoadableSingleton {

    private static readonly string FastGrowthIconName = "trend-growth-fast";
    private static readonly string ModerateGrowthIconName = "trend-growth-moderate";
    private static readonly string SlowGrowthIconName = "trend-growth-slow";
    private static readonly string FastDepletionIconName = "trend-depletion-fast";
    private static readonly string ModerateDepletionIconName = "trend-depletion-moderate";
    private static readonly string SlowDepletionIconName = "trend-depletion-slow";
    private readonly IAssetLoader _assetLoader;
    private Sprite _fastGrowthIcon;
    private Sprite _moderateGrowthIcon;
    private Sprite _slowGrowthIcon;
    private Sprite _fastDepletionIcon;
    private Sprite _moderateDepletionIcon;
    private Sprite _slowDepletionIcon;

    public TrendIconProvider(IAssetLoader assetLoader) {
      _assetLoader = assetLoader;
    }

    public Sprite GetIcon(TrendType trendType) {
      return trendType switch {
          TrendType.HighGrowth => _fastGrowthIcon,
          TrendType.MediumGrowth => _moderateGrowthIcon,
          TrendType.LowGrowth => _slowGrowthIcon,
          TrendType.HighDepletion => _fastDepletionIcon,
          TrendType.MediumDepletion => _moderateDepletionIcon,
          TrendType.LowDepletion => _slowDepletionIcon,
          TrendType.Stable => null,
          _ => throw new ArgumentOutOfRangeException(nameof(trendType), trendType, null)
      };
    }

    public void Load() {
      _fastGrowthIcon = _assetLoader.Load<Sprite>(GetIconPath(FastGrowthIconName));
      _moderateGrowthIcon = _assetLoader.Load<Sprite>(GetIconPath(ModerateGrowthIconName));
      _slowGrowthIcon = _assetLoader.Load<Sprite>(GetIconPath(SlowGrowthIconName));
      _fastDepletionIcon = _assetLoader.Load<Sprite>(GetIconPath(FastDepletionIconName));
      _moderateDepletionIcon = _assetLoader.Load<Sprite>(GetIconPath(ModerateDepletionIconName));
      _slowDepletionIcon = _assetLoader.Load<Sprite>(GetIconPath(SlowDepletionIconName));
    }

    private static string GetIconPath(string iconName) {
      return $"UI/Images/GoodStatistics/{iconName}";
    }

  }
}