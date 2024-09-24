using ModSettings.Core;
using ModSettings.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CommonUI {
  public class TextInputBaseFieldModSettingElement<T> : IModSettingElement {

    public VisualElement Root { get; }
    public ModSetting ModSetting { get; }
    private readonly TextInputBaseField<T> _textInputBaseField;

    public TextInputBaseFieldModSettingElement(VisualElement root,
                                               ModSetting modSetting,
                                               TextInputBaseField<T> textInputBaseField) {
      Root = root;
      ModSetting = modSetting;
      _textInputBaseField = textInputBaseField;
    }

    public bool ShouldBlockInput =>
        _textInputBaseField.focusController?.focusedElement == _textInputBaseField;
    
  }
}