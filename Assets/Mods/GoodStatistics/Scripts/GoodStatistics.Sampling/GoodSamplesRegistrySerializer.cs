using Timberborn.Goods;
using Timberborn.Persistence;

namespace GoodStatistics.Sampling {
  public class GoodSamplesRegistrySerializer : IObjectSerializer<GoodSamplesRegistry> {

    private static readonly ListKey<GoodSampleRecords> GoodSampleRecordsKey =
        new("GoodSampleRecords");
    private readonly GoodSampleRecordsSerializer _goodSampleRecordsSerializer;
    private readonly IGoodService _goodService;

    public GoodSamplesRegistrySerializer(GoodSampleRecordsSerializer goodSampleRecordsSerializer,
                                         IGoodService goodService) {
      _goodSampleRecordsSerializer = goodSampleRecordsSerializer;
      _goodService = goodService;
    }

    public void Serialize(GoodSamplesRegistry value, IObjectSaver objectSaver) {
      objectSaver.Set(GoodSampleRecordsKey, value.GoodSampleRecords, _goodSampleRecordsSerializer);
    }

    public Obsoletable<GoodSamplesRegistry> Deserialize(IObjectLoader objectLoader) {
      var goodSamplesRegistry = GoodSamplesRegistry.CreateFromSave(
          objectLoader.Get(GoodSampleRecordsKey, _goodSampleRecordsSerializer));
      foreach (var goodId in _goodService.Goods) {
        if (!goodSamplesRegistry.HasGood(goodId)) {
          goodSamplesRegistry.AddMissingGood(goodId);
        }
      }
      return new(goodSamplesRegistry);
    }

  }
}