using UnityEngine;
using UnityEngine.UIElements;

namespace Minimap.CoreUI {
  internal class MinimapCameraFrustum : VisualElement {

    private readonly float _thickness;
    private float _scale = 1;

    private MinimapCameraFrustum(float thickness) {
      _thickness = thickness;
    }

    public static MinimapCameraFrustum Create(float thickness) {
      var frustum = new MinimapCameraFrustum(thickness) {
          style = { position = Position.Absolute, width = 0, height = 0 }
      };
      frustum.generateVisualContent += frustum.OnGenerateVisualContent;
      return frustum;
    }

    public void SetScale(float scale) {
      var clampedScale = Mathf.Clamp(scale, 0.05f, 1);
      if (!Mathf.Approximately(clampedScale, _scale)) {
        _scale = clampedScale;
        MarkDirtyRepaint();
      }
    }

    private void OnGenerateVisualContent(MeshGenerationContext mgc) {
      var paint2D = mgc.painter2D;

      paint2D.strokeColor = Color.white;
      paint2D.lineCap = LineCap.Round;
      paint2D.lineJoin = LineJoin.Round;
      paint2D.lineWidth = _thickness;
      paint2D.BeginPath();
      paint2D.MoveTo(new(-128 * _scale, 64 * _scale));
      paint2D.LineTo(new(128 * _scale, 64 * _scale));
      paint2D.LineTo(new(80 * _scale, -64 * _scale));
      paint2D.LineTo(new(-80 * _scale, -64 * _scale));
      paint2D.ClosePath();
      paint2D.Stroke();
    }

  }
}