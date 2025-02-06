using System.Collections.Generic;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.GameDistricts;
using Timberborn.SingletonSystem;

namespace GoodStatistics.BatchControl {
  internal class GoodStatisticsBatchControlTab : BatchControlTab {

    private readonly DistrictCenterRegistry _districtCenterRegistry;
    private readonly GoodStatisticsBatchControlRowGroupFactory
        _goodStatisticsBatchControlRowGroupFactory;

    public GoodStatisticsBatchControlTab(VisualElementLoader visualElementLoader,
                                         BatchControlDistrict batchControlDistrict,
                                         DistrictCenterRegistry districtCenterRegistry,
                                         GoodStatisticsBatchControlRowGroupFactory
                                             goodStatisticsBatchControlRowGroupFactory,
                                         EventBus eventBus) : base(
        visualElementLoader, batchControlDistrict, eventBus) {
      _districtCenterRegistry = districtCenterRegistry;
      _goodStatisticsBatchControlRowGroupFactory = goodStatisticsBatchControlRowGroupFactory;
    }

    public override string TabNameLocKey => "eMka.GoodStatistics.BatchControlTabName";
    public override string TabImage => "GoodStatistics";
    public override string BindingKey => "GoodStatisticsTab";
    public override bool RemoveEmptyRowGroups => true;

    public override IEnumerable<BatchControlRowGroup> GetRowGroups(IEnumerable<EntityComponent>
                                                                       entities) {
      yield return _goodStatisticsBatchControlRowGroupFactory.CreateGlobal();
      foreach (var districtCenter in _districtCenterRegistry.FinishedDistrictCenters) {
        yield return _goodStatisticsBatchControlRowGroupFactory.Create(districtCenter);
      }
    }

  }
}