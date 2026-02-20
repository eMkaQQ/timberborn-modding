using Timberborn.Common;
using Timberborn.Goods;
using UnityEngine;

namespace Riverborne.Core {
  public class RaftCargoBoxAndBarrel {

    private static readonly string Box1Name = ".Box";
    private static readonly string Box2Name = ".Box2";
    private static readonly string Barrel1Name = ".Barrel";
    private static readonly string Barrel2Name = ".Barrel2";
    private readonly GoodIconVisualizer _goodIconVisualizer;
    private readonly CargoObject _box1;
    private readonly CargoObject _box2;
    private readonly CargoObject _barrel1;
    private readonly CargoObject _barrel2;
    private readonly bool _tryShowBoth;

    private RaftCargoBoxAndBarrel(GoodIconVisualizer goodIconVisualizer,
                                  CargoObject box1,
                                  CargoObject box2,
                                  CargoObject barrel1,
                                  CargoObject barrel2,
                                  bool tryShowBoth) {
      _goodIconVisualizer = goodIconVisualizer;
      _box1 = box1;
      _box2 = box2;
      _barrel1 = barrel1;
      _barrel2 = barrel2;
      _tryShowBoth = tryShowBoth;
    }

    public static RaftCargoBoxAndBarrel Create(GoodIconVisualizer goodIconVisualizer,
                                               GameObject root, string rowName, bool tryShowBoth) {
      var box1 = CargoObject.Create(root.FindChild(rowName + Box1Name), VisibleContainer.Box);
      var box2 = CargoObject.Create(root.FindChild(rowName + Box2Name), VisibleContainer.Box);
      var barrel1 =
          CargoObject.Create(root.FindChild(rowName + Barrel1Name), VisibleContainer.Barrel);
      var barrel2 =
          CargoObject.Create(root.FindChild(rowName + Barrel2Name), VisibleContainer.Barrel);
      return tryShowBoth
          ? new(goodIconVisualizer, box1, box2, barrel1, barrel2, true)
          : new(goodIconVisualizer, box2, box1, barrel2, barrel1, false);
    }

    public void Hide() {
      _box1.Hide();
      _box2.Hide();
      _barrel1.Hide();
      _barrel2.Hide();
    }

    public bool TryShow(RaftCargo cargo) {
      if (TryShow(cargo, _box1, _barrel1, out var primaryShown)) {
        if (primaryShown) {
          TryShow(cargo, _barrel1, _box2, out _);
        } else {
          TryShow(cargo, _box1, _barrel2, out _);
        }
        return true;
      }
      return false;
    }

    private bool TryShow(RaftCargo cargo, CargoObject primaryObject,
                         CargoObject secondaryObject, out bool primaryShown) {
      foreach (var raftCargoItem in cargo.Items) {
        if (TryShowObject(primaryObject, raftCargoItem)) {
          primaryShown = true;
          cargo.MoveToLast(raftCargoItem);
          return true;
        }
        if (!_tryShowBoth && TryShowObject(secondaryObject, raftCargoItem)) {
          primaryShown = false;
          cargo.MoveToLast(raftCargoItem);
          return true;
        }
      }
      primaryShown = false;
      foreach (var raftCargoItem in cargo.Items) {
        if (TryShowObject(secondaryObject, raftCargoItem)) {
          cargo.MoveToLast(raftCargoItem);
          return true;
        }
      }
      return false;
    }

    private bool TryShowObject(CargoObject cargoObject, RaftCargoItem cargoItem) {
      if (cargoObject.Container == cargoItem.GoodSpec.VisibleContainer) {
        cargoObject.Show();
        _goodIconVisualizer.ShowIcon(cargoObject.Material, cargoItem.GoodSpec);
        return true;
      }
      return false;
    }

    private class CargoObject {

      public Material Material { get; }
      public VisibleContainer Container { get; }
      private readonly GameObject _root;

      private CargoObject(GameObject root,
                          Material material,
                          VisibleContainer container) {
        _root = root;
        Material = material;
        Container = container;
      }

      public static CargoObject Create(GameObject root, VisibleContainer container) {
        var meshRenderer = root.GetComponent<MeshRenderer>();
        return new(root, meshRenderer.material, container);
      }

      public void Show() {
        _root.SetActive(true);
      }

      public void Hide() {
        _root.SetActive(false);
      }

    }

  }
}