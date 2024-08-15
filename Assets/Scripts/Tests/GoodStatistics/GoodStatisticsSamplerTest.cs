using GoodStatistics.Core;
using GoodStatistics.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;
using Timberborn.GameDistricts;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.Persistence;
using Timberborn.ResourceCountingSystem;
using Timberborn.TimeSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.GoodStatistics {
  public class GoodStatisticsSamplerTest {

    private static readonly string Bread = "Bread";
    private static readonly string Log = "Log";
    private static readonly string Water = "Water";
    private Inventory _inventory1;
    private Inventory _inventory2;
    private DistrictCenter _districtCenter1;
    private DistrictCenter _districtCenter2;

    [SetUp]
    public void Setup() {
      var gameObject = new GameObject();
      _inventory1 = gameObject.AddComponent<Inventory>();
      var gameObject2 = new GameObject();
      _inventory2 = gameObject2.AddComponent<Inventory>();
      var gameObject3 = new GameObject();
      _districtCenter1 = gameObject3.AddComponent<DistrictCenter>();
      var gameObject4 = new GameObject();
      _districtCenter2 = gameObject4.AddComponent<DistrictCenter>();
    }

    [TearDown]
    public void TearDown() {
      Object.DestroyImmediate(_inventory1.GameObjectFast);
      Object.DestroyImmediate(_inventory2.GameObjectFast);
      Object.DestroyImmediate(_districtCenter1.GameObjectFast);
      Object.DestroyImmediate(_districtCenter2.GameObjectFast);
    }

    [Test]
    public void ShouldCollectGlobalSample() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);

      // When
      _inventory1.Give(new(Bread, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();

      // Then
      var sample = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      Assert.AreEqual(1, sample.InputOutputStock);
      Assert.AreEqual(100, sample.InputOutputCapacity);
      Assert.AreEqual(1 / 100f, sample.FillRate);
    }

    [Test]
    public void ShouldCollectEmptyInventorySample() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);

      // When
      _inventory1.Give(new(Bread, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();
      _inventory1.Take(new(Bread, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();

      // Then
      var sample0 = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      var sample1 = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[1];
      Assert.AreEqual(0, sample0.InputOutputStock);
      Assert.AreEqual(100, sample0.InputOutputCapacity);
      Assert.AreEqual(0, sample0.FillRate);
      Assert.AreEqual(1, sample1.InputOutputStock);
      Assert.AreEqual(100, sample1.InputOutputCapacity);
      Assert.AreEqual(1 / 100f, sample1.FillRate);
    }

    [Test]
    public void ShouldRemoveSampleAboveMaxSamples() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);

      // When
      for (var i = 0; i < GoodStatisticsConstants.MaxSamples + 1; i++) {
        _inventory1.Give(new(Bread, 1));
        resourceCountingService.Tick();
        goodStatisticsSampler.CollectGoodsSamples();
      }

      // Then
      var sample = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples;
      Assert.AreEqual(GoodStatisticsConstants.MaxSamples, sample.Count);
      Assert.AreEqual(GoodStatisticsConstants.MaxSamples + 1, sample[0].InputOutputStock);
      Assert.AreEqual(2, sample[^1].InputOutputStock);
    }

    [Test]
    public void ShouldCollectAllGoodsSamples() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100),
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Log), 100),
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Water), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);

      // When
      _inventory1.Give(new(Bread, 1));
      _inventory1.Give(new(Log, 1));
      _inventory1.Give(new(Water, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();

      // Then
      var sampleBread = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      var sampleLog = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Log).GoodSamples[0];
      var sampleWater = globalResourceCountsRegistry.ResourceCountsRegistry.ResourceCountHistories
          .Single(history => history.GoodId == Water).GoodSamples[0];
      Assert.AreEqual(1, sampleBread.InputOutputStock);
      Assert.AreEqual(1, sampleLog.InputOutputStock);
      Assert.AreEqual(1, sampleWater.InputOutputStock);
    }

    [Test]
    public void ShouldCollectDistrictSample() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);
      _inventory2.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Log), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory2);
      var districtBuilding = _inventory1.GameObjectFast.AddComponent<DistrictBuilding>();
      districtBuilding.AssignInstantDistrict(_districtCenter1);
      _inventory2.GameObjectFast.AddComponent<DistrictBuilding>();

      var districtResourceCountsRegistry =
          _districtCenter1.GameObjectFast.AddComponent<DistrictResourceCountsRegistry>();
      var goodService = new DummyGoodService();
      districtResourceCountsRegistry.InjectDependencies(goodStatisticsSampler,
                                                        new(new(new(goodService), new(new())),
                                                            goodService), goodService);
      districtResourceCountsRegistry.Awake();
      districtResourceCountsRegistry.InitializeEntity();
      districtResourceCountsRegistry.OnEnterFinishedState();

      // When
      _inventory1.Give(new(Bread, 1));
      _inventory2.Give(new(Log, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();

      // Then
      var breadDistrictSample = districtResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      var logDistrictSample = districtResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Log).GoodSamples[0];
      var breadGlobalSample = globalResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      var logGlobalSample = globalResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Log).GoodSamples[0];
      Assert.AreEqual(1, breadDistrictSample.InputOutputStock);
      Assert.AreEqual(0, logDistrictSample.InputOutputStock);
      Assert.AreEqual(1, breadGlobalSample.InputOutputStock);
      Assert.AreEqual(1, logGlobalSample.InputOutputStock);
    }

    [Test]
    public void ShouldCollectSampleFromAllDistricts() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);
      _inventory2.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Log), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory2);
      var districtBuilding = _inventory1.GameObjectFast.AddComponent<DistrictBuilding>();
      districtBuilding.AssignInstantDistrict(_districtCenter1);
      var districtBuilding2 = _inventory2.GameObjectFast.AddComponent<DistrictBuilding>();
      districtBuilding2.AssignInstantDistrict(_districtCenter2);

      var districtResourceCountsRegistry1 =
          _districtCenter1.GameObjectFast.AddComponent<DistrictResourceCountsRegistry>();
      var goodService = new DummyGoodService();
      districtResourceCountsRegistry1.InjectDependencies(goodStatisticsSampler,
                                                         new(new(new(goodService), new(new())),
                                                             goodService), goodService);
      districtResourceCountsRegistry1.Awake();
      districtResourceCountsRegistry1.InitializeEntity();
      districtResourceCountsRegistry1.OnEnterFinishedState();

      var districtResourceCountsRegistry2 =
          _districtCenter2.GameObjectFast.AddComponent<DistrictResourceCountsRegistry>();
      districtResourceCountsRegistry2.InjectDependencies(goodStatisticsSampler,
                                                         new(new(new(goodService), new(new())),
                                                             goodService), goodService);
      districtResourceCountsRegistry2.Awake();
      districtResourceCountsRegistry2.InitializeEntity();
      districtResourceCountsRegistry2.OnEnterFinishedState();

      // When
      _inventory1.Give(new(Bread, 1));
      _inventory2.Give(new(Log, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();

      // Then
      var breadDistrictSample1 = districtResourceCountsRegistry1.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      var logDistrictSample2 = districtResourceCountsRegistry2.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Log).GoodSamples[0];
      var breadGlobal = globalResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples[0];
      var logGlobal = globalResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Log).GoodSamples[0];
      Assert.AreEqual(1, breadDistrictSample1.InputOutputStock);
      Assert.AreEqual(1, logDistrictSample2.InputOutputStock);
      Assert.AreEqual(1, breadGlobal.InputOutputStock);
      Assert.AreEqual(1, logGlobal.InputOutputStock);
    }

    [Test]
    public void ShouldCollectSampleAfterInventorySwitchedDistrict() {
      // Given
      CreateServices(out var inventoryService,
                     out var resourceCountingService,
                     out var goodStatisticsSampler,
                     out var globalResourceCountsRegistry);

      _inventory1.Initialize("test", 100,
                             new[] {
                                 new StorableGoodAmount(
                                     StorableGood.CreateGiveableAndTakeable(Bread), 100)
                             }, true, true, false, new DummyGoodDisallower());
      inventoryService.Add(_inventory1);
      var districtBuilding = _inventory1.GameObjectFast.AddComponent<DistrictBuilding>();

      var districtResourceCountsRegistry1 =
          _districtCenter1.GameObjectFast.AddComponent<DistrictResourceCountsRegistry>();
      var goodService = new DummyGoodService();
      districtResourceCountsRegistry1.InjectDependencies(goodStatisticsSampler,
                                                         new(new(new(goodService), new(new())),
                                                             goodService), goodService);
      districtResourceCountsRegistry1.Awake();
      districtResourceCountsRegistry1.InitializeEntity();
      districtResourceCountsRegistry1.OnEnterFinishedState();

      var districtResourceCountsRegistry2 =
          _districtCenter2.GameObjectFast.AddComponent<DistrictResourceCountsRegistry>();
      districtResourceCountsRegistry2.InjectDependencies(goodStatisticsSampler,
                                                         new(new(new(goodService), new(new())),
                                                             goodService), goodService);
      districtResourceCountsRegistry2.Awake();
      districtResourceCountsRegistry2.InitializeEntity();
      districtResourceCountsRegistry2.OnEnterFinishedState();

      // When
      _inventory1.Give(new(Bread, 1));
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();
      districtBuilding.AssignInstantDistrict(_districtCenter1);
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();
      districtBuilding.AssignInstantDistrict(_districtCenter2);
      resourceCountingService.Tick();
      goodStatisticsSampler.CollectGoodsSamples();

      // Then
      var district1Samples = districtResourceCountsRegistry1.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples;
      var district2Samples = districtResourceCountsRegistry2.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples;
      var globalSamples = globalResourceCountsRegistry.ResourceCountsRegistry
          .ResourceCountHistories
          .Single(history => history.GoodId == Bread).GoodSamples;
      Assert.AreEqual(0, district1Samples[0].InputOutputStock);
      Assert.AreEqual(1, district1Samples[1].InputOutputStock);
      Assert.AreEqual(0, district1Samples[2].InputOutputStock);
      Assert.AreEqual(1, district2Samples[0].InputOutputStock);
      Assert.AreEqual(0, district2Samples[1].InputOutputStock);
      Assert.AreEqual(0, district2Samples[2].InputOutputStock);
      Assert.AreEqual(1, globalSamples[0].InputOutputStock);
      Assert.AreEqual(1, globalSamples[1].InputOutputStock);
      Assert.AreEqual(1, globalSamples[2].InputOutputStock);
    }

    private static void CreateServices(out InventoryService inventoryService,
                                       out ResourceCountingService resourceCountingService,
                                       out GoodStatisticsSampler goodStatisticsSampler,
                                       out GlobalResourceCountsRegistry
                                           globalResourceCountsRegistry) {
      var goodService = new DummyGoodService();
      globalResourceCountsRegistry = new(goodService, new DummySingletonLoader(),
                                         new(new(new(goodService), new(new())), goodService));
      globalResourceCountsRegistry.Load();
      inventoryService = new();
      resourceCountingService = new();
      resourceCountingService.InjectDependencies(inventoryService, new());
      var districtContextService = new DistrictContextService(new());
      goodStatisticsSampler = new(globalResourceCountsRegistry,
                                  resourceCountingService,
                                  districtContextService,
                                  goodService, new(), new DayNightCycleMock());
    }

    private class DummyGoodService : IGoodService {

      private readonly List<string> _goods = new() { Bread, Log, Water };

      public ReadOnlyList<string> Goods => new(_goods);

      public bool HasGood(string id) {
        throw new NotSupportedException();
      }

      public GoodSpecification GetGoodOrNull(string id) {
        throw new NotSupportedException();
      }

      public GoodSpecification GetGood(string id) {
        throw new NotSupportedException();
      }

      public IEnumerable<string> GetGoodsForGroup(string groupId) {
        throw new NotSupportedException();
      }

    }

    private class DummySingletonLoader : ISingletonLoader {

      public IObjectLoader GetSingleton(SingletonKey key) {
        throw new NotSupportedException();
      }

      public bool HasSingleton(SingletonKey key) {
        return false;
      }

    }

    private class DummyGoodDisallower : IGoodDisallower {

      public event EventHandler<DisallowedGoodsChangedEventArgs> DisallowedGoodsChanged {
        add { }
        remove { }
      }

      public int AllowedAmount(string goodId) {
        return 100;
      }

    }

    private class DayNightCycleMock : IDayNightCycle {

      public int ConfiguredDayLengthInSeconds { get; }
      public int DayNumber { get; }
      public float DaytimeLengthInHours { get; }
      public float NighttimeLengthInHours { get; }
      public float HoursPassedToday { get; }
      public float DayProgress { get; }
      public float PartialDayNumber { get; }
      public TimeOfDay FluidTimeOfDay { get; }
      public bool IsDaytime { get; }
      public bool IsNighttime { get; }
      public float FixedDeltaTimeInHours { get; }

      public float DayNumberHoursFromNow(float hours) {
        throw new NotSupportedException();
      }

      public (float start, float end) BoundsInHours(TimeOfDay timeOfDay) {
        throw new NotSupportedException();
      }

      public float HoursToNextStartOf(TimeOfDay timeOfDay) {
        throw new NotSupportedException();
      }

      public float SecondsToHours(float seconds) {
        throw new NotSupportedException();
      }

      public float FluidHoursToNextStartOf(TimeOfDay timeOfDay) {
        throw new NotSupportedException();
      }

      public void SetTimeToNextDay() {
        throw new NotSupportedException();
      }

      public void JumpTimeInHours(float hours) {
        throw new NotSupportedException();
      }

    }

  }
}