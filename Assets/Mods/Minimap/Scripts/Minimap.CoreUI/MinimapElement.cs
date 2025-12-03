using Minimap.Core;
using ModSettings.CoreUI;
using System.Linq;
using Timberborn.CameraSystem;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.MapStateSystem;
using Timberborn.Modding;
using Timberborn.SingletonSystem;
using Timberborn.UILayoutSystem;
using Timberborn.UISound;
using UnityEngine;
using UnityEngine.UIElements;

namespace Minimap.CoreUI {
  internal class MinimapElement : ILoadableSingleton,
                                  IUpdatableSingleton {

    private static readonly int BackgroundMargin = 7;
    private readonly UILayout _uiLayout;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly MinimapTexture _minimapTexture;
    private readonly ModSettingsBox _modSettingsBox;
    private readonly ModRepository _modRepository;
    private readonly CameraService _cameraService;
    private readonly MapSize _mapSize;
    private readonly InputService _inputService;
    private readonly UISoundController _uiSoundController;
    private float _mapScale;
    private MinimapCameraFrustum _cameraFrustum;
    private VisualElement _root;
    private VisualElement _background;
    private Image _minimapImage;
    private bool _isDragging;
    private Mod _minimapMod;

    public MinimapElement(UILayout uiLayout,
                          VisualElementLoader visualElementLoader,
                          MinimapTexture minimapTexture,
                          ModSettingsBox modSettingsBox,
                          ModRepository modRepository,
                          CameraService cameraService,
                          MapSize mapSize,
                          InputService inputService,
                          UISoundController uiSoundController) {
      _uiLayout = uiLayout;
      _visualElementLoader = visualElementLoader;
      _minimapTexture = minimapTexture;
      _modSettingsBox = modSettingsBox;
      _modRepository = modRepository;
      _cameraService = cameraService;
      _mapSize = mapSize;
      _inputService = inputService;
      _uiSoundController = uiSoundController;
    }

    public void Load() {
      if (_minimapTexture.MinimapEnabled) {
        CreateVisualElements();
        _minimapMod = _modRepository.EnabledMods.Single(mod => mod.Manifest.Id == "eMka.Minimap");
        var mapSize = _mapSize.TerrainSize.x > _mapSize.TerrainSize.y
            ? _mapSize.TerrainSize.x
            : _mapSize.TerrainSize.y;
        _mapScale = 2f / mapSize;
        SetMinimapSize();
      }
    }

    public void UpdateSingleton() {
      if (_minimapTexture.MinimapEnabled) {
        if (_isDragging) {
          if (!_inputService.MainMouseButtonHeld) {
            _isDragging = false;
          } else {
            DragCamera();
          }
        }
        UpdateCameraFrustum();
      }
    }

    public void SetMinimapRotation(int rotation) {
      if (_minimapTexture.MinimapEnabled) {
        var isPerpendicular = rotation is 90 or 270;
        _background.style.rotate = Quaternion.Euler(0, 0, rotation);
        _root.style.height = isPerpendicular
            ? new(_minimapImage.style.width.value.value + BackgroundMargin, LengthUnit.Pixel)
            : new Length(_minimapImage.style.height.value.value + BackgroundMargin,
                         LengthUnit.Pixel);
        _root.style.width = isPerpendicular
            ? new(_minimapImage.style.height.value.value + BackgroundMargin, LengthUnit.Pixel)
            : new Length(_minimapImage.style.width.value.value + BackgroundMargin,
                         LengthUnit.Pixel);
      }
    }

    private void CreateVisualElements() {
      _root = _visualElementLoader.LoadVisualElement("Minimap/Minimap");
      _background = _root.Q<VisualElement>("MinimapBackground");
      _minimapImage = _root.Q<Image>("MinimapImage");
      _minimapImage.image = _minimapTexture.Texture;
      _uiLayout.AddBottomRight(_root, 100);
      _root.RegisterCallback<MouseDownEvent>(OnMouseDown);
      _cameraFrustum = MinimapCameraFrustum.Create(2);
      _root.Q<Image>("MinimapImage").Add(_cameraFrustum);
    }

    private void OnMouseDown(MouseDownEvent evt) {
      if (evt.button == 0) {
        _isDragging = true;
      } else if (evt.button == 1) {
        _modSettingsBox.Open(_minimapMod);
        _uiSoundController.PlayClickSound();
      }
    }

    private void SetMinimapSize() {
      var baseMinimapSize = 256f;
      if (_mapSize.TerrainSize.x > _mapSize.TerrainSize.y) {
        _minimapImage.style.width = new Length(baseMinimapSize, LengthUnit.Pixel);
        _minimapImage.style.height =
            new Length(baseMinimapSize * _mapSize.TerrainSize.y / _mapSize.TerrainSize.x,
                       LengthUnit.Pixel);
      } else {
        _minimapImage.style.width =
            new Length(baseMinimapSize * _mapSize.TerrainSize.x / _mapSize.TerrainSize.y,
                       LengthUnit.Pixel);
        _minimapImage.style.height = new Length(baseMinimapSize, LengthUnit.Pixel);
      }
      _background.style.width =
          new Length(_minimapImage.style.width.value.value + BackgroundMargin, LengthUnit.Pixel);
      _background.style.height =
          new Length(_minimapImage.style.height.value.value + BackgroundMargin, LengthUnit.Pixel);
    }

    private void DragCamera() {
      var mousePosNdc = _inputService.MousePositionNdc;
      var rootRect = _minimapImage.panel.visualTree.worldBound;
      var rootMousePos = new Vector2(mousePosNdc.x * rootRect.width,
                                     (1 - mousePosNdc.y) * rootRect.height);
      var localPos = _minimapImage.WorldToLocal(rootMousePos);
      localPos.x = Mathf.Clamp(localPos.x, 0, _minimapImage.contentRect.width);
      localPos.y = Mathf.Clamp(localPos.y, 0, _minimapImage.contentRect.height);
      var cameraXPos = localPos.x / _minimapImage.contentRect.width;
      var cameraYPos = localPos.y / _minimapImage.contentRect.height;
      var targetX = cameraXPos * _mapSize.TotalSize.x;
      var targetZ = (1 - cameraYPos) * _mapSize.TotalSize.y;
      _cameraService.MoveTargetTo(new(targetX, 0, targetZ));
    }

    private void UpdateCameraFrustum() {
      var zoomLevel = _cameraService.ZoomLevel;
      var visibleTiles = Mathf.Pow(0.5f * zoomLevel + 3, 2);
      _cameraFrustum.SetScale(visibleTiles * _mapScale);
      _cameraFrustum.style.rotate =
          Quaternion.Euler(0, 0, _cameraService.HorizontalAngle + 180);
      var cameraXPos = _cameraService.Target.x / _mapSize.TotalSize.x;
      var cameraYPos = _cameraService.Target.z / _mapSize.TotalSize.y;
      _cameraFrustum.style.bottom = new Length(cameraYPos * 100, LengthUnit.Percent);
      _cameraFrustum.style.left = new Length(cameraXPos * 100, LengthUnit.Percent);
    }

  }
}