using System;
using JetBoxer2D.Engine.Input.Objects;

namespace JetBoxer2D.Engine.Input.InputTypes;

//Input that varies between a singular value (e.g. from -1 to 1 or from 0 to 1, etc...)
public class ValueInput : BaseInput
{
    public float Value
    {
        get
        {
            if (_keyboardValue != 0)
                return _keyboardValue;
            if (_mouseValue != 0)
                return _mouseValue;
            if (_gamepadValue != 0)
                return _gamepadValue;
            return 0;
        }
    }

    private float _keyboardValue;
    private float _mouseValue;
    private float _gamepadValue;

    //Call this method through the InputMaps.UpdateInput(), to update this input's axis values
    public void UpdateKeyboardValue(float value)
    {
        //E.g. AxisInput MoveHor.KeyboardValue = -1 aka player moves to the left

        _keyboardValue = Math.Clamp(value, -100f, 100f);
    }

    public void UpdateMouseValue(float value)
    {
        _mouseValue = Math.Clamp(value, -100f, 100f);
    }

    public void UpdateGamepadValue(float value)
    {
        _gamepadValue = Math.Clamp(value, -100f, 100f);
    }
}