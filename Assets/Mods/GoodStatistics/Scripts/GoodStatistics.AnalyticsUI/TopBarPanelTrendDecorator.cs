using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace GoodStatistics.AnalyticsUI {
  internal class TopBarPanelTrendDecorator {

    private readonly GoodTrendElementFactory _goodTrendElementFactory;
    private readonly ISingletonRepository _singletonRepository;

    public TopBarPanelTrendDecorator(GoodTrendElementFactory goodTrendElementFactory,
                                     ISingletonRepository singletonRepository) {
      _goodTrendElementFactory = goodTrendElementFactory;
      _singletonRepository = singletonRepository;
    }

    public IEnumerable<GoodTrendElement> CreateTrendElements() {
      var topBarPanel = _singletonRepository.GetSingletons<IUpdatableSingleton>()
          .SingleOrDefault(obj => obj.GetType().FullName == "Timberborn.TopBarSystem.TopBarPanel");
      if (topBarPanel != null) {
        return CreateTrendElements(topBarPanel);
      }
      throw new("IUpdatableSingleton TopBarPanel not found");
    }

    private IEnumerable<GoodTrendElement> CreateTrendElements(object topBarPanel) {
      var countersField = GetPrivateField(topBarPanel, "_counters");
      var counters = (IList) countersField.GetValue(topBarPanel);
      return CreateTrendElements(counters);
    }

    private IEnumerable<GoodTrendElement> CreateTrendElements(IList counters) {
      foreach (var counter in counters) {
        if (TryGetPrivateField(counter, "_root", out var rootField)) {
          yield return CreateTrendElement(rootField, counter);
        } else {
          var counterRowsField = GetPrivateField(counter, "_counterRows");
          var counterRows = (IList) counterRowsField.GetValue(counter);
          foreach (var counterRow in counterRows) {
            var rowRootField = GetPrivateField(counterRow, "_root");
            yield return CreateTrendElement(rowRootField, counterRow);
          }
        }
      }
    }

    private static FieldInfo GetPrivateField(object owner, string fieldName) {
      if (TryGetPrivateField(owner, fieldName, out var field)) {
        return field;
      }
      throw new($"Field '{fieldName}' not found in {owner.GetType().FullName}");
    }

    private static bool TryGetPrivateField(object owner, string fieldName,
                                           out FieldInfo rootField) {
      rootField = owner.GetType().GetField(fieldName,
                                           BindingFlags.Instance | BindingFlags.NonPublic);
      return rootField != null;
    }

    private GoodTrendElement CreateTrendElement(FieldInfo rootField, object counter) {
      var root = (VisualElement) rootField.GetValue(counter);
      var goodIdField = GetPrivateField(counter, "_goodId");
      var goodId = (string) goodIdField.GetValue(counter);
      var trendElement = _goodTrendElementFactory.Create(goodId);
      root.Q<VisualElement>("Icon").Add(trendElement.Root);
      return trendElement;
    }

  }
}