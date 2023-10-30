using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Engine.Input.Objects;

namespace Super_Duper_Shooter.Engine.Input.InputTypes;

public class AxisInput : BaseInput
{
    /// <summary>
    /// Whether the X Axis is currently positive or negative
    /// </summary>
    public bool CurrentX => _currentX;
    
    /// <summary>
    /// Whether the Y Axis is currently positive or negative
    /// </summary>
    public bool CurrentY => _currentY;
    
    private Vector2 _keyboardValue;
    private Vector2 _mouseValue;
    private Vector2 _gamepadValue;

    //Value that store a boolean of whether either X or Y value of the Axis is currently positive or negative
    private bool _currentX;
    private bool _currentY;
    
    //Total value of the axis
    public Vector2 Value
    {
        get//Select one of the active inputs
        {
            if (_keyboardValue != Vector2.Zero) //keyboard has priority
                return _keyboardValue;
            if (_mouseValue != Vector2.Zero)
                return _mouseValue;
            if (_gamepadValue != Vector2.Zero)
                return _gamepadValue;
            
            return Vector2.Zero;//return 0 when no input is detected
        }
    }
    
    #region Keyboard
    public void UpdateKeyboardValueX(bool negativeX, bool positiveX)
    {
        _keyboardValue.X = negativeX ? -1 : _keyboardValue.X;
        _keyboardValue.X = positiveX ? 1 : _keyboardValue.X;
        
        if (negativeX || positiveX)
            _currentX = true;
        else
            _currentX = false;
        
        if (!_currentX)//if no input is received, reset the X value of the axis
            _keyboardValue.X = 0;
    }
    
    public void UpdateKeyboardValueY(bool negativeY, bool positiveY)
    {
        if (negativeY || positiveY)
            _currentY = true;
        else
            _currentY = false;
        
        _keyboardValue.Y = negativeY ? -1 : _keyboardValue.Y;
        _keyboardValue.Y = positiveY ? 1 : _keyboardValue.Y;
        
        if (!_currentY)
            _keyboardValue.Y = 0;
    }
    #endregion

    #region Mouse
    public void UpdateMouseValueX(bool negativeX, bool positiveX)
    {
        _mouseValue.X = negativeX ? -1 : _mouseValue.X;
        _mouseValue.X = positiveX ? 1 : _mouseValue.X;
        
        if (negativeX || positiveX)
            _currentX = true;
        else
            _currentX = false;
        
        if (!_currentX)//if no input is received, reset the X value of the axis
            _mouseValue.X = 0;
    }
    
    public void UpdateMouseValueY(bool negativeY, bool positiveY)
    {
        if (negativeY || positiveY)
            _currentY = true;
        else
            _currentY = false;
        
        _mouseValue.Y = negativeY ? -1 : _mouseValue.Y;
        _mouseValue.Y = positiveY ? 1 : _mouseValue.Y;
        
        if (!_currentY)
            _mouseValue.Y = 0;
    }
    #endregion

    #region Gamepad
    public void UpdateGamepadValueX(bool negativeX, bool positiveX)
    {
        if (negativeX || positiveX)
            _currentX = true;
        else
            _currentX = false;
        
        _gamepadValue.X = negativeX ? -1 : _gamepadValue.X;;
        _gamepadValue.X = positiveX ? 1 : _gamepadValue.X;;
        
        if (!_currentX)
            _gamepadValue.X = 0;
    }
    
    public void UpdateGamepadValueY(bool negativeY, bool positiveY)
    {
        if (negativeY || positiveY)
            _currentY = true;
        else
            _currentY = false;
        
        _gamepadValue.Y = negativeY ? -1 : _gamepadValue.Y;
        _gamepadValue.Y = positiveY ? 1 : _gamepadValue.Y;
        
        if (!_currentY)
            _gamepadValue.Y = 0;
    }
    #endregion
}