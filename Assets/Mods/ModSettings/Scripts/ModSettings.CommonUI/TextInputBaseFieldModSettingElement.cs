using ModSettings.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  public class TextInputBaseFieldModSettingElement<T> : IModSettingElement {

    public VisualElement Root { get; }
    private readonly TextInputBaseField<T> _textInputBaseField;

    public TextInputBaseFieldModSettingElement(VisualElement root,
                                               TextInputBaseField<T> textInputBaseField) {
      Root = root;
      _textInputBaseField = textInputBaseField;
    }

    public bool ShouldBlockInput =>
        _textInputBaseField.focusController?.focusedElement == _textInputBaseField;

  }
}