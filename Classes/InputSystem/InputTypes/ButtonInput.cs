using Super_Duper_Shooter.Classes.InputSystem.Base;

namespace Super_Duper_Shooter.Classes.InputSystem.InputTypes;

public class ButtonInput : BaseInput
{
    //Input Values
    // public bool KeyboardValue => _keyboardValue; //For example, AxisInput.Keyboard = new Vector2(Keys.A, Keys.D)
    // public bool MouseValue => _mouseValue;
    // public bool GamepadValue => _gamepadValue;

    public bool Pressed
    {
        get 
        {
        if (_keyboardPressed) //keyboard has priority
            return _keyboardPressed;
        if (_mousePressed)
            return _mousePressed;
        if (_gamepadPressed)
            return _gamepadPressed;
            
        return false;//return 0 when no input is detected
        
        }
    }

    private bool _keyboardPressed;
    private bool _mousePressed;
    private bool _gamepadPressed;

    private bool _pressed;
    //Call these methods through the InputMaps.UpdateInput(), to update this input's axis values

    public void UpdateKeyboardValue(bool button)
    {
        _keyboardPressed = button;
    }

    public void UpdateMouseValue(bool button)
    {
        _mousePressed = button;
    }

    public void UpdateGamepadValue(bool button)
    {
        _gamepadPressed = button;
    }
}