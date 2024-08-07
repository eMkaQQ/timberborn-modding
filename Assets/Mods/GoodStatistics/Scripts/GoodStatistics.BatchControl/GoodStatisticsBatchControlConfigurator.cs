using Bindito.Core;
using Timberborn.BatchControl;

namespace GoodStatistics.BatchControl {
  [Context("Game")]
  public class GoodStatisticsBatchControlConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition) {
      containerDefinition.Bind<GoodStatisticsBatchControlRowGroupFactory>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsBatchControlTab>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsGroupFactory>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsRowItemFactory>().AsSingleton();
      containerDefinition.Bind<GoodStatisticsBatchControlItemFactory>().AsSingleton();

      containerDefinition.MultiBind<BatchControlModule>()
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