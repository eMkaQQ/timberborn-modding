using System;
using System.Collections.Generic;
using Timberborn.Common;

namespace GoodStatistics.Sampling {
  public class GoodSamplesRegistry {

    public event EventHandler<GoodSampleRecords> SampleAdded;
    private readonly Dictionary<string, GoodSampleRecords> _goodSamplesRecordsMap = new();
    private readonly List<GoodSampleRecords> _goodSamplesRecords = new();

    private GoodSamplesRegistry() {
    }

    public static GoodSamplesRegistry CreateNew(IReadOnlyList<string> goodIds) {
      var goodSamplesRegistry = new GoodSamplesRegistry();
      foreach (var goodId in goodIds) {
        goodSamplesRegistry.AddMissingGood(goodId);
      }
      return goodSamplesRegistry;
    }

    public static GoodSamplesRegistry CreateFromSave(List<GoodSampleRecords> goodSamplesRecords) {
      var goodSamplesRegistry = new GoodSamplesRegistry();
      foreach (var goodSampleRecords in goodSamplesRecords) {
        goodSamplesRegistry._goodSamplesRecordsMap[goodSampleRecords.GoodId] = goodSampleRecords;
        goodSamplesRegistry._goodSamplesRecords.Add(goodSampleRecords);
      }
      return goodSamplesRegistry;
    }

    public ReadOnlyList<GoodSampleRecords> GoodSampleRecords => new(_goodSamplesRecords);

    public void AddSample(string goodId, GoodSample goodSample) {
      var history = _goodSamplesRecordsMap[goodId];
      history.Add(goodSample);
      SampleAdded?.Invoke(this, history);
    }

    public bool HasGood(string goodId) {
      return _goodSamplesRecordsMap.ContainsKey(goodId);
    }

    public GoodSampleRecords GetGoodSampleRecords(string goodId) {
      return _goodSamplesRecordsMap[goodId];
    }

    public void AddMissingGood(string goodId) {
      var goodSampleRecords = Sampling.GoodSampleRecords.CreateNew(goodId);
      _goodSamplesRecordsMap[goodId] = goodSampleRecords;
      _goodSamplesRecords.Add(goodSampleRecords);
    }

  }
}