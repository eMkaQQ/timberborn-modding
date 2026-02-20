using Riverborne.Core;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Riverborne.CoreUI {
  public class EditDispatchPanelGood {

    private static readonly string ChoosenGoodClass = "edit-dispatch-panel-good--chosen";
    public string GoodId { get; }
    private readonly AlternateClickableFactory _alternateClickableFactory;
    private readonly int _weight;
    private readonly VisualElement _root;
    private int _minAmount;
    private IntegerField _amountField;
    private Button _minusButton;
    private AlternateClickable _alternateMinus;
    private AlternateClickable _alternatePlus;

    public EditDispatchPanelGood(AlternateClickableFactory alternateClickableFactory,
                                 string goodId,
                                 int weight,
                                 VisualElement root) {
      _alternateClickableFactory = alternateClickableFactory;
      GoodId = goodId;
      _weight = weight;
      _root = root;
    }

    public int Amount => _amountField.value;

    public void Initialize() {
      _minAmount = GoodId == Raft.MaterialGood ? Raft.MaterialAmount : 0;
      _amountField = _root.Q<IntegerField>("Amount");
      _minusButton = _root.Q<Button>("MinusButton");

      TextFields.InitializeIntegerField(_amountField, _minAmount,
                                        afterEditingCallback: _ => SetAmount(_amountField.value));
      _alternateMinus = _alternateClickableFactory.Create(_minusButton, () => ChangeAmount(-1),
                                                          () => ChangeAmount(-10));
      _alternatePlus = _alternateClickableFactory.Create(_root.Q<Button>("PlusButton"),
                                                         () => ChangeAmount(1),
                                                         () => ChangeAmount(10));
      UpdateVisualElements();
    }

    public void Update() {
      _alternateMinus.Update();
      _alternatePlus.Update();
    }

    public void SetAmount(int newAmount) {
      var clampedAmount = Mathf.Clamp(newAmount, _minAmount, EditDispatchPanel.MaxTotalWeight);
      _amountField.SetValueWithoutNotify(clampedAmount);
      UpdateVisualElements();
    }

    public int GetWeight() {
      return Amount * _weight;
    }

    private void ChangeAmount(int delta) {
      var newAmount = _amountField.value + delta;
      SetAmount(newAmount);
    }

    private void UpdateVisualElements() {
      var amount = _amountField.value;
      _minusButton.SetEnabled(amount > _minAmount);
      _root.EnableInClassList(ChoosenGoodClass, amount > 0);
    }

  }
}