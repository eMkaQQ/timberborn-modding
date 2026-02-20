using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.Goods;
using Timberborn.GoodsUI;
using Timberborn.Localization;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  internal class EditDispatchPanelGoodFactory {

    private static readonly string GoodWeightLocKey = "Riverborne.EditDispatchPanel.Weight";
    private readonly GoodsGroupSpecService _goodsGroupSpecService;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly IGoodService _goodService;
    private readonly GoodDescriber _goodDescriber;
    private readonly ILoc _loc;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly AlternateClickableFactory _alternateClickableFactory;

    public EditDispatchPanelGoodFactory(GoodsGroupSpecService goodsGroupSpecService,
                                        VisualElementLoader visualElementLoader,
                                        IGoodService goodService,
                                        GoodDescriber goodDescriber,
                                        ILoc loc,
                                        ITooltipRegistrar tooltipRegistrar,
                                        AlternateClickableFactory alternateClickableFactory) {
      _goodsGroupSpecService = goodsGroupSpecService;
      _visualElementLoader = visualElementLoader;
      _goodService = goodService;
      _goodDescriber = goodDescriber;
      _loc = loc;
      _tooltipRegistrar = tooltipRegistrar;
      _alternateClickableFactory = alternateClickableFactory;
    }

    public IEnumerable<EditDispatchPanelGood> CreateGoods(VisualElement parent) {
      foreach (var goodGroupSpec in _goodsGroupSpecService.GoodGroupSpecs) {
        foreach (var good in CreateGroup(parent, goodGroupSpec)) {
          yield return good;
        }
      }
    }

    private IEnumerable<EditDispatchPanelGood> CreateGroup(VisualElement parent,
                                                           GoodGroupSpec goodGroupSpec) {
      var elementName = "Riverborne/EditDispatchPanelGoodGroup";
      var groupElement = _visualElementLoader.LoadVisualElement(elementName);
      groupElement.Q<Image>("Icon").sprite = goodGroupSpec.Icon.Asset;
      foreach (var goodId in _goodService.GetGoodsForGroup(goodGroupSpec.Id)) {
        yield return CreateGood(groupElement.Q<VisualElement>("Goods"), goodId);
      }
      parent.Add(groupElement);
    }

    private EditDispatchPanelGood CreateGood(VisualElement parent, string goodId) {
      var good = _goodService.GetGood(goodId);
      var elementName = "Riverborne/EditDispatchPanelGood";
      var goodElement = _visualElementLoader.LoadVisualElement(elementName);
      var describedGood = _goodDescriber.GetDescribedGood(goodId);
      var icon = goodElement.Q<Image>("GoodIcon");
      icon.sprite = describedGood.Icon;
      _tooltipRegistrar.Register(icon, describedGood.DisplayName);
      goodElement.Q<Label>("WeightLabel").text = _loc.T(GoodWeightLocKey, good.Weight);
      parent.Add(goodElement);
      var editDispatchPanelGood = new EditDispatchPanelGood(_alternateClickableFactory,
                                                            goodId, good.Weight, goodElement);
      editDispatchPanelGood.Initialize();
      return editDispatchPanelGood;
    }

  }
}