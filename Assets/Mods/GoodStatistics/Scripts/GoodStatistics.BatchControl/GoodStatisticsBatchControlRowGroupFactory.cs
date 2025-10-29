using GoodStatistics.Sampling;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.GameDistricts;
using Timberborn.GameDistrictsBatchControl;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlRowGroupFactory {

    private readonly BatchControlRowGroupFactory _batchControlRowGroupFactory;
    private readonly DistrictCenterRowItemFactory _districtCenterRowItemFactory;
    private readonly GoodStatisticsRowItemFactory _goodStatisticsRowItemFactory;
    private readonly VisualElementLoader _visualElementLoader;

    public GoodStatisticsBatchControlRowGroupFactory(
        BatchControlRowGroupFactory batchControlRowGroupFactory,
        DistrictCenterRowItemFactory districtCenterRowItemFactory,
        GoodStatisticsRowItemFactory goodStatisticsRowItemFactory,
        VisualElementLoader visualElementLoader) {
      _batchControlRowGroupFactory = batchControlRowGroupFactory;
      _districtCenterRowItemFactory = districtCenterRowItemFactory;
      _goodStatisticsRowItemFactory = goodStatisticsRowItemFactory;
      _visualElementLoader = visualElementLoader;
    }

    public BatchControlRowGroup Create(DistrictCenter districtCenter) {
      var elementName = "Game/BatchControl/BatchControlRow";
      var root = _visualElementLoader.LoadVisualElement(elementName);

      var districtGoodSamplesRegistry =
          districtCenter.GetComponent<DistrictGoodSamplesRegistry>();
      var districtCenterRowItem = _districtCenterRowItemFactory.Create(districtCenter);
      var row = new BatchControlRow(root, districtCenter.GetComponent<EntityComponent>(),
                                    districtCenterRowItem);
      var goodStatisticsRowGroup = _batchControlRowGroupFactory.CreateUnsorted(row);
      goodStatisticsRowGroup.AddRow(
          _goodStatisticsRowItemFactory.Create(districtGoodSamplesRegistry));
      return goodStatisticsRowGroup;
    }

    public BatchControlRowGroup CreateGlobal() {
      var goodStatisticsRowGroup = _batchControlRowGroupFactory.CreateUnsorted(new(new()));
      goodStatisticsRowGroup.AddRow(_goodStatisticsRowItemFactory.CreateGlobal());
      return goodStatisticsRowGroup;
    }

  }
}