using Riverborne.Core;
using System;
using System.Collections.Generic;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.Goods;
using Timberborn.GoodsUI;
using Timberborn.Localization;
using Timberborn.TimeSystem;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  internal class RaftDispatchItemFactory {

    private static readonly string HoursShortLocKey = "Time.HoursShort";
    private static readonly string PausedLocKey = "Riverborne.DispatchItem.Paused";
    private static readonly string PausedClass = "paused";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly GoodDescriber _goodDescriber;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly IDayNightCycle _dayNightCycle;
    private readonly ILoc _loc;
    private readonly Dictionary<VisualElement, RaftDispatch> _raftDispatchItems = new();
    private Action<RaftDispatch> _editDispatch;
    private Action<RaftDispatch> _removeDispatch;
    private Action<RaftDispatch> _toggleDispatchPause;

    public RaftDispatchItemFactory(VisualElementLoader visualElementLoader,
                                   GoodDescriber goodDescriber,
                                   ITooltipRegistrar tooltipRegistrar,
                                   IDayNightCycle dayNightCycle,
                                   ILoc loc) {
      _visualElementLoader = visualElementLoader;
      _goodDescriber = goodDescriber;
      _tooltipRegistrar = tooltipRegistrar;
      _dayNightCycle = dayNightCycle;
      _loc = loc;
    }

    public void Initialize(Action<RaftDispatch> editDispatch,
                           Action<RaftDispatch> removeDispatch,
                           Action<RaftDispatch> pauseDispatch) {
      Asserts.FieldIsNull(this, _editDispatch, nameof(_editDispatch));
      _editDispatch = editDispatch;
      _removeDispatch = removeDispatch;
      _toggleDispatchPause = pauseDispatch;
    }

    public void Clear() {
      _raftDispatchItems.Clear();
    }

    public VisualElement Create() {
      var root = _visualElementLoader.LoadVisualElement("Riverborne/RaftDispatchItem");
      var editButton = root.Q<Button>("EditButton");
      editButton.RegisterCallback<ClickEvent>(_ => _editDispatch(_raftDispatchItems[root]));
      var removeButton = root.Q<Button>("RemoveButton");
      removeButton.RegisterCallback<ClickEvent>(_ => _removeDispatch(_raftDispatchItems[root]));
      var pauseButton = root.Q<Image>("PauseButton");
      pauseButton.RegisterCallback<ClickEvent>(_ => _toggleDispatchPause(_raftDispatchItems[root]));
      return root;
    }

    public void Bind(VisualElement element, RaftDispatch raftDispatch) {
      _raftDispatchItems[element] = raftDispatch;
      element.Q<Label>("Name").text = raftDispatch.Name;
      var goodsParent = element.Q<VisualElement>("Goods");
      goodsParent.Clear();
      foreach (var goodAmount in raftDispatch.Cargo) {
        CreateGood(goodAmount, goodsParent);
      }
    }

    public void Unbind(VisualElement element) {
      _raftDispatchItems.Remove(element);
    }

    public void Update() {
      foreach (var (element, raftDispatch) in _raftDispatchItems) {
        var progressBar = element.Q<Timberborn.CoreUI.ProgressBar>("IntervalProgress");
        var label = element.Q<Label>("Progress");
        var pauseButton = element.Q<Image>("PauseButton");
        pauseButton.EnableInClassList(PausedClass, raftDispatch.IsPaused);
        if (raftDispatch.IsPaused) {
          progressBar.SetProgress(0);
          label.text = _loc.T(PausedLocKey);
        } else {
          var cooldown = _dayNightCycle.PartialDayNumber - raftDispatch.LastDispatchTime;
          var dayTimeInterval = raftDispatch.DayTimeInterval;
          var cooldownClamped = Math.Max(0, cooldown);
          var progress = dayTimeInterval == 0 ? 1 : cooldownClamped / dayTimeInterval;
          progressBar.SetProgress(progress);
          var hoursLeft = (dayTimeInterval - cooldownClamped) * 24f;
          label.text = _loc.T(HoursShortLocKey, hoursLeft.ToString("F1"));
        }
      }
    }

    private void CreateGood(GoodAmount goodAmount, VisualElement parent) {
      var root = _visualElementLoader.LoadVisualElement("Riverborne/RaftDispatchItemGood");

      var describedGood = _goodDescriber.GetDescribedGood(goodAmount.GoodId);
      _tooltipRegistrar.Register(root, describedGood.DisplayName);

      root.Q<Image>("GoodIcon").sprite = describedGood.Icon;
      root.Q<Label>("Amount").text = goodAmount.Amount.ToString();
      parent.Add(root);
    }

  }
}