using System;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.Localization;
using UnityEngine;

namespace ModSettings.ColorPicker {
  public class ColorPickerShower : IInputProcessor {

    private readonly DialogBoxShower _dialogBoxShower;
    private readonly ColorPickerFactory _colorPickerFactory;
    private readonly ILoc _loc;
    private readonly InputService _inputService;
    private ColorPicker _colorPicker;
    private Action<Color> _onColorSelected;

    public ColorPickerShower(DialogBoxShower dialogBoxShower,
                             ColorPickerFactory colorPickerFactory,
                             ILoc loc,
                             InputService inputService) {
      _dialogBoxShower = dialogBoxShower;
      _colorPickerFactory = colorPickerFactory;
      _loc = loc;
      _inputService = inputService;
    }

    public void ShowColorPicker(Color initialColor, bool useAlpha, Action<Color> onColorSelected) {
      Asserts.FieldIsNull(this, _colorPicker, nameof(_colorPicker));
      _colorPicker = _colorPickerFactory.Create(initialColor, useAlpha);
      _onColorSelected = onColorSelected;
      _dialogBoxShower.Create()
          .AddContent(_colorPicker.Root)
          .SetConfirmButton(CallbackAndClear, _loc.T(CommonLocKeys.OKKey))
          .SetCancelButton(Clear, _loc.T(CommonLocKeys.CancelKey))
          .Show();
      _inputService.AddInputProcessor(this);
    }

    public bool ProcessInput() {
      return _colorPicker.UpdateInput();
    }

    private void CallbackAndClear() {
      _onColorSelected?.Invoke(_colorPicker.ChosenColor);
      Clear();
    }

    private void Clear() {
      _colorPicker.Clear();
      _colorPicker = null;
      _inputService.RemoveInputProcessor(this);
    }

  }
}