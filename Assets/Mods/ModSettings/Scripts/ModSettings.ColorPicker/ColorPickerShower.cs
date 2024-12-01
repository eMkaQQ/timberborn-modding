using System;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModSettings.ColorPicker {
  public class ColorPickerShower : IUpdatableSingleton {

    private readonly DialogBoxShower _dialogBoxShower;
    private readonly ColorPickerFactory _colorPickerFactory;
    private readonly ILoc _loc;
    private ColorPicker _colorPicker;
    private Action<Color> _onColorSelected;

    public ColorPickerShower(DialogBoxShower dialogBoxShower,
                             ColorPickerFactory colorPickerFactory,
                             ILoc loc) {
      _dialogBoxShower = dialogBoxShower;
      _colorPickerFactory = colorPickerFactory;
      _loc = loc;
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
    }

    public void UpdateSingleton() {
      _colorPicker?.Update();
    }

    private void CallbackAndClear() {
      _onColorSelected?.Invoke(_colorPicker.ChosenColor);
      Clear();
    }

    private void Clear() {
      _colorPicker.Clear();
      _colorPicker = null;
    }

  }
}