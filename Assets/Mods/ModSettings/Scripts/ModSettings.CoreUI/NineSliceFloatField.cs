using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ModSettings.CoreUI {
  [UxmlElement]
  internal partial class NineSliceFloatField : FloatField {

    private readonly NineSliceBackground _nineSliceBackground = new();

    // ReSharper disable once MemberCanBePrivate.Global
    public NineSliceFloatField() {
      generateVisualContent += OnGenerateVisualContent;
      RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
    }

    private void OnCustomStyleResolved(CustomStyleResolvedEvent e) {
      _nineSliceBackground.GetDataFromStyle(customStyle);
      MarkDirtyRepaint();
    }

    private void OnGenerateVisualContent(MeshGenerationContext mgc) {
      _nineSliceBackground.GenerateVisualContent(mgc, paddingRect);
    }

  }
}