using Bindito.Core;
using Timberborn.BatchControl;

namespace GoodStatistics.BatchControl {
  [Context("Game")]
  public class GoodStatisticsBatchControlConfigurator : Configurator {

    protected override void Configure() {
      Bind<GoodStatisticsBatchControlRowGroupFactory>().AsSingleton();
      Bind<GoodStatisticsBatchControlTab>().AsSingleton();
      Bind<GoodStatisticsGroupFactory>().AsSingleton();
      Bind<GoodStatisticsRowItemFactory>().AsSingleton();
      Bind<GoodStatisticsBatchControlItemFactory>().AsSingleton();

      MultiBind<BatchControlModule>()
          .ToProvider<BatchControlModuleProvider>()
          .AsSingleton();
    }

    private class BatchControlModuleProvider : IProvider<BatchControlModule> {

      private readonly GoodStatisticsBatchControlTab _goodStatisticsBatchControlTab;

      public BatchControlModuleProvider(GoodStatisticsBatchControlTab
                                            goodStatisticsBatchControlTab) {
        _goodStatisticsBatchControlTab = goodStatisticsBatchControlTab;
      }

      public BatchControlModule Get() {
        var builder = new BatchControlModule.Builder();
        builder.AddTab(_goodStatisticsBatchControlTab, 90);
        return builder.Build();
      }

    }

  }
}