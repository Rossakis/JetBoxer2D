using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Classes.InputEvents.Base;

namespace Super_Duper_Shooter.Classes.InputEvents.InputTypes;

public class AxisInput : BaseInput
{
    public Vector2 KeyboardValue => _keyboardValue; //For example, AxisInput.Keyboard = new Vector2(Keys.A, Keys.D)
    public Vector2 MouseValue => _mouseValue;
    public Vector2 GamepadValue => _gamepadValue;

    private Vector2 _keyboardValue;
    private Vector2 _mouseValue;
    private Vector2 _gamepadValue;


    //Call this method through the InputMaps.UpdateInput(), to update this input's axis values
    public void UpdateKeyboardValue(bool buttonA, bool buttonB)
    {
        //E.g. AxisInput MoveHor.KeyboardValue = Vector2(1,0), aka player moves to the left

        _keyboardValue.X = buttonA ? 1 : 0;
        _keyboardValue.Y = buttonB ? 1 : 0;
    }

    public void UpdateMouseValue(bool buttonA, bool buttonB)
    {
        _mouseValue.X = buttonA ? 1 : 0;
        _mouseValue.Y = buttonB ? 1 : 0;
    }

    public void UpdateGamepadValue(bool buttonA, bool buttonB)
    {
        _gamepadValue.X = buttonA ? 1 : 0;
        _gamepadValue.Y = buttonB ? 1 : 0;
    }
}