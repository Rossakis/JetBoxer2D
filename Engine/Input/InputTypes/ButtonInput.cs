using Super_Duper_Shooter.Engine.Input.Objects;

namespace Super_Duper_Shooter.Engine.Input.InputTypes;

public class ButtonInput : BaseInput
{
    public bool Pressed
    {
        get 
        {
        if (_isKeyboardPressed) //keyboard has priority
            return _isKeyboardPressed;
        if (_isMousePressed)
            return _isMousePressed;
        if (_isGamepadPressed)
            return _isGamepadPressed;
            
        return false;//return 0 when no input is detected
        
        }
    }

    public bool PressedDown
    {
        get
        {
            if (_isKeyboardPressedDown)
                return true;
            if (_isMousePressedDown)
                return true;
            if (_isGamepadPressedDown)
                return true;
            
            return false;
        }
    }
    
    private bool _isKeyboardPressed;//for Pressed property
    private bool _wasKeyboardPressed;//flag for marking the previous button pressed state
    private bool _isKeyboardPressedDown;//for PressedDown property
    
    private bool _isMousePressed;
    private bool _wasMousePressed;
    private bool _isMousePressedDown;
    
    private bool _isGamepadPressed;
    private bool _wasGamepadPressed;
    private bool _isGamepadPressedDown;
    
    public void UpdateKeyboardValue(bool button)
    {
        //Normal press
        _isKeyboardPressed = button;
        
        //Press down
        if (!_wasKeyboardPressed && _isKeyboardPressed)
            _isKeyboardPressedDown = true;
        else
            _isKeyboardPressedDown = false;
        
        _wasKeyboardPressed = _isKeyboardPressed;//update the previous state of the input for future reference
    }

    public void UpdateMouseValue(bool button)
    {
        //Normal press
        _isMousePressed = button;
        
        //Press down
        if (!_wasMousePressed && _isMousePressed)
            _isMousePressedDown = true;
        else
            _isMousePressedDown = false;
        
        _wasMousePressed = _isMousePressed;
    }
    
    public void UpdateGamepadValue(bool button)
    {
        //Normal press
        _isGamepadPressed = button;
        
        //Press down
        if (!_wasGamepadPressed && _isGamepadPressed)
            _isGamepadPressedDown = true;
        else
            _isGamepadPressedDown = false;
        
        _wasGamepadPressed = _isGamepadPressed;
    }
}