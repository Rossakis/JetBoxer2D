using Super_Duper_Shooter.Classes.InputEvents.Base;

namespace Super_Duper_Shooter.Classes.InputEvents.InputTypes;

public class ButtonInput : BaseInput
{
    //Input Values
    public bool KeyboardValue => _keyboardValue; //For example, AxisInput.Keyboard = new Vector2(Keys.A, Keys.D)
    public bool MouseValue => _mouseValue;
    public bool GamepadValue => _gamepadValue;

    private bool _keyboardValue;
    private bool _mouseValue;
    private bool _gamepadValue;


    //Call these methods through the InputMaps.UpdateInput(), to update this input's axis values

    public void UpdateKeyboardValue(bool button)
    {
        _keyboardValue = button;
        //E.g. AxisInput MoveHor.KeyboardValue = Vector2(1,0), aka player moves to the left
    }

    public void UpdateMouseValue(bool button)
    {
        _mouseValue = button;
    }

    public void UpdateGamepadValue(bool button)
    {
        _gamepadValue = button;
    }
}