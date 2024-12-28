using System;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace Minimap.CoreUI {
  internal class MinimapElementRotator : ISaveableSingleton,
                                         ILoadableSingleton,
                                         IPostLoadableSingleton {

    private static readonly SingletonKey MinimapElementRotatorKey = new("MinimapElementRotator");
    private static readonly PropertyKey<int> RotationKey = new("Rotation");
    private readonly ISingletonLoader _singletonLoader;
    private readonly MinimapElement _minimapElement;
    private MinimapDirection _minimapDirection;

    public MinimapElementRotator(ISingletonLoader singletonLoader,
                                 MinimapElement minimapElement) {
      _singletonLoader = singletonLoader;
      _minimapElement = minimapElement;
    }

    public void Save(ISingletonSaver singletonSaver) {
      var minimapElementRotator = singletonSaver.GetSingleton(MinimapElementRotatorKey);
      minimapElementRotator.Set(RotationKey, (int) _minimapDirection);
    }

    public void Load() {
      if (_singletonLoader.HasSingleton(MinimapElementRotatorKey)) {
        var rotation = _singletonLoader.GetSingleton(MinimapElementRotatorKey).Get(RotationKey);
        _minimapDirection = (MinimapDirection) rotation;
      }
    }

    public void PostLoad() {
      SetElementRotation();
    }

    public void RotateClockwise() {
      _minimapDirection = GetNextDirection(true);
      SetElementRotation();
    }

    public void RotateCounterClockwise() {
      _minimapDirection = GetNextDirection(false);
      SetElementRotation();
    }

    public void Reset() {
      _minimapDirection = MinimapDirection.North;
      SetElementRotation();
    }

    private void SetElementRotation() {
      _minimapElement.SetMinimapRotation((int) _minimapDirection);
    }

    private MinimapDirection GetNextDirection(bool clockwise) {
      return _minimapDirection switch {
          MinimapDirection.North => clockwise ? MinimapDirection.East : MinimapDirection.West,
          MinimapDirection.East => clockwise ? MinimapDirection.South : MinimapDirection.North,
          MinimapDirection.South => clockwise ? MinimapDirection.West : MinimapDirection.East,
          MinimapDirection.West => clockwise ? MinimapDirection.North : MinimapDirection.South,
          _ => throw new ArgumentOutOfRangeException()
      };
    }

    private enum MinimapDirection {

      North = 0,
      East = 90,
      South = 180,
      West = 270

    }

  }
}