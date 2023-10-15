using Super_Duper_Shooter.Classes.InputEvents.Base;

namespace Super_Duper_Shooter.Classes.InputEvents.InputTypes;

//Input that varies between a singular value (e.g. from -1 to 1 or from 0 to 1, etc...)
public class ValueInput : BaseInput
{
    public int KeyboardValue => _keyboardValue; //For example, AxisInput.Keyboard = new Vector2(Keys.A, Keys.D)
    public int MouseValue => _mouseValue;
    public int GamepadValue => _gamepadValue;

    private int _keyboardValue;
    private int _mouseValue;
    private int _gamepadValue;


    //Call this method through the InputMaps.UpdateInput(), to update this input's axis values
    public void UpdateKeyboardValue(bool value)
    {
        //E.g. AxisInput MoveHor.KeyboardValue = -1 aka player moves to the left

        _keyboardValue = value ? 1 : 0;
    }

    public void UpdateMouseValue(bool value)
    {
        _mouseValue = value ? 1 : 0;
    }

    public void UpdateGamepadValue(bool value)
    {
        _gamepadValue = value ? 1 : 0;
    }
}