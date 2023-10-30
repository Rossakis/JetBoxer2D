using System;
using Super_Duper_Shooter.Engine.Input.Objects;

namespace Super_Duper_Shooter.Engine.Input.InputTypes;

//Input that varies between a singular value (e.g. from -1 to 1 or from 0 to 1, etc...)
public class ValueInput : BaseInput
{
    public int Value
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
    
    private int _keyboardValue;
    private int _mouseValue;
    private int _gamepadValue;

    //Call this method through the InputMaps.UpdateInput(), to update this input's axis values
    public void UpdateKeyboardValue(int value)
    {
        //E.g. AxisInput MoveHor.KeyboardValue = -1 aka player moves to the left

        _keyboardValue = Math.Clamp(value, -1, 1);


    }

    public void UpdateMouseValue(int value)
    {
        _mouseValue = Math.Clamp(value, -1, 1);
    }

    public void UpdateGamepadValue(int value)
    {
        _gamepadValue = Math.Clamp(value, -1, 1);
    }
}