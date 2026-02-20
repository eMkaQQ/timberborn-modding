using System;
using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.MapIndexSystem;
using Timberborn.Navigation;
using Timberborn.WaterSystem;
using TimberPhysics.Core;
using TimberPhysics.Layers;
using UnityEngine;

namespace TimberPhysics.Water {
  public class FloatingObject : BaseComponent,
                                IAwakableComponent,
                                IPhysicalObject,
                                IPhysicalObjectLayerProvider {

    private readonly ThreadSafeWaterMap _threadSafeWaterMap;
    private readonly MapIndexService _mapIndexService;
    private readonly EntityService _entityService;
    private FloatingObjectSpec _spec;
    private Rigidbody _rigidbody;
    private float _defaultLinearDamping;
    private float _defaultAngularDamping;
    private readonly List<SubmergedPoint> _submergedPoints = new();

    public FloatingObject(ThreadSafeWaterMap threadSafeWaterMap,
                          MapIndexService mapIndexService,
                          EntityService entityService) {
      _threadSafeWaterMap = threadSafeWaterMap;
      _mapIndexService = mapIndexService;
      _entityService = entityService;
    }

    public string LayerName => FloatingObjectLayerRegistrar.LayerName;

    public void Awake() {
      _spec = GetComponent<FloatingObjectSpec>();
      _rigidbody = GetComponent<RigidbodyAttacher>().Rigidbody;
      _defaultLinearDamping = _rigidbody.linearDamping;
      _defaultAngularDamping = _rigidbody.angularDamping;
      foreach (var collider in GameObject.GetComponentsInChildren<Collider>()) {
        collider.material = _spec.PhysicsMaterial.Asset;
      }
    }

    public void PhysicsStep(float deltaTime) {
      UpdateSubmergedPoints(out var weightSum, out var currentSum, out var shouldBeDeleted);

      if (shouldBeDeleted) {
        _entityService.Delete(this);
        return;
      }

      ApplyVerticalForce(weightSum);

      if (_submergedPoints.Count > 0) {
        var waterCurrentXZ = currentSum / _submergedPoints.Count;
        var current = new Vector3(waterCurrentXZ.x, 0f, waterCurrentXZ.y);
        _rigidbody.AddForce(current * _spec.CurrentForceMultiplier, ForceMode.Acceleration);
        ApplyWaterDrag();
      } else {
        ResetDrag();
      }

      _submergedPoints.Clear();
    }

    private void UpdateSubmergedPoints(out float weightSum, out Vector2 currentSum,
                                       out bool shouldBeDeleted) {
      weightSum = 0;
      currentSum = Vector2.zero;
      shouldBeDeleted = false;
      for (var index = 0; index < _spec.FloatingPoints.Length; index++) {
        var point = _spec.FloatingPoints[index];
        var position = point.Position;
        var worldPosition = Transform.TransformPoint(new(position.x, 0f, position.y));
        var gridPosition = NavigationCoordinateSystem.WorldToGridInt(worldPosition);
        if (!Sizing.SizeContains(_mapIndexService.TotalSize, gridPosition)) {
          shouldBeDeleted = true;
          break;
        }

        var waterHeight = _threadSafeWaterMap.WaterHeightOrFloor(gridPosition);
        var depth = waterHeight - worldPosition.y;
        if (depth > 0f) {
          weightSum += point.Weight;
          var columnIndex = GetColumnIndexForCorner(gridPosition, worldPosition.y);
          currentSum += _threadSafeWaterMap.FlowDirections[columnIndex];
          _submergedPoints.Add(new(worldPosition, point.IsCenterPoint, point.Weight, waterHeight));
        }
      }
    }

    private int GetColumnIndexForCorner(Vector3Int gridPosition, float cornerHeight) {
      var columnCounts = _threadSafeWaterMap.ColumnCounts;
      var index = _mapIndexService.CoordinatesToIndex3D(new(gridPosition.x, gridPosition.y));
      for (var i = 0; i < columnCounts[index]; i++) {
        var index3D = i * _mapIndexService.VerticalStride + index;
        ref readonly var column = ref _threadSafeWaterMap.WaterColumns[index3D];
        if (cornerHeight < column.Ceiling) {
          return index3D;
        }
      }
      throw new InvalidOperationException(
          $"No column found for grid position {gridPosition} at height {cornerHeight}");
    }

    private void ApplyVerticalForce(float weightSum) {
      if (weightSum <= 0f) {
        return;
      }

      var g = -Physics.gravity.y;

      for (var i = 0; i < _submergedPoints.Count; i++) {
        var submergedPoint = _submergedPoints[i];
        var targetY = submergedPoint.WaterHeight - _spec.Immersion;
        var pointY = submergedPoint.WorldPosition.y;
        var error = targetY - pointY;
        var velY = _rigidbody.GetPointVelocity(submergedPoint.WorldPosition).y;
        var accelY = g + error * _spec.PointStiffness - velY * _spec.PointDamping;
        var force = Vector3.up * (accelY * (submergedPoint.Weight / weightSum));

        if (submergedPoint.IsCenterPoint) {
          _rigidbody.AddForce(force, ForceMode.Acceleration);
        } else {
          _rigidbody.AddForceAtPosition(force, submergedPoint.WorldPosition,
                                        ForceMode.Acceleration);
        }
      }
    }

    private void ApplyWaterDrag() {
      _rigidbody.linearDamping = _spec.WaterLinearDamping;
      _rigidbody.angularDamping = _spec.WaterAngularDamping;
    }

    private void ResetDrag() {
      _rigidbody.linearDamping = _defaultLinearDamping;
      _rigidbody.angularDamping = _defaultAngularDamping;
    }

    private readonly struct SubmergedPoint {

      public Vector3 WorldPosition { get; }
      public bool IsCenterPoint { get; }
      public float Weight { get; }
      public float WaterHeight { get; }

      public SubmergedPoint(Vector3 worldPosition,
                            bool isCenterPoint,
                            float weight,
                            float waterHeight) {
        WorldPosition = worldPosition;
        IsCenterPoint = isCenterPoint;
        Weight = weight;
        WaterHeight = waterHeight;
      }

    }

  }
}