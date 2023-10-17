using System;
using Microsoft.Xna.Framework.Input;

namespace Super_Duper_Shooter.Classes.Extensions;

/// <summary>
/// A wrapper class for Mouse Input, so that we input information from it in the same way we do for Keyboard and Gamepad
///classes (e.g. Mouse.IsButtonDown(MouseButton.RightButton)
/// </summary>

public enum MouseButtons : short
{
    RightButton = 0,
    MiddleButton = 1,
    LeftButton = 2
}

public enum MouseAxis : short
{
    Horizontal = 0,
    Vertical = 1
}

public class MouseInput
{
    private MouseState _previousMouseState;
    private MouseState _currentMouseState;

    private static MouseInput _instance;

    public static MouseInput Instance
    {
        get //if instance = null, create new
        {
            if (_instance == null)
                throw new Exception("MouseInput.Instance wasn't instantiated");

            return _instance;
        }
        set => _instance = value;
    }

    public static void Update()
    {
        _instance._previousMouseState = _instance._currentMouseState;
        _instance._currentMouseState = Mouse.GetState();
    }

    public static bool IsButtonPressed(MouseButtons button)
    {
        switch (button)
        {
            case MouseButtons.LeftButton:
                return _instance._currentMouseState.LeftButton == ButtonState.Pressed;
            case MouseButtons.MiddleButton:
                return _instance._currentMouseState.MiddleButton == ButtonState.Pressed;
            case MouseButtons.RightButton:
                return _instance._currentMouseState.RightButton == ButtonState.Pressed;
            // Add cases for other buttons if needed
            default:
                return false;
        }
    }

    public static bool IsButtonReleased(MouseButtons button)
    {
        switch (button)
        {
            case MouseButtons.LeftButton:
                return _instance._currentMouseState.LeftButton == ButtonState.Released &&
                       _instance._previousMouseState.LeftButton == ButtonState.Pressed;
            case MouseButtons.MiddleButton:
                return _instance._currentMouseState.MiddleButton == ButtonState.Released &&
                       _instance._previousMouseState.MiddleButton == ButtonState.Pressed;
            case MouseButtons.RightButton:
                return _instance._currentMouseState.RightButton == ButtonState.Released &&
                       _instance._previousMouseState.RightButton == ButtonState.Pressed;
            // Add cases for other buttons if needed
            default:
                return false;
        }
    }

    /// <summary>
    /// Was the button pressed for at least one frame
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool WasButtonPressed(MouseButtons button)
    {
        switch (button)
        {
            case MouseButtons.LeftButton:
                return _instance._currentMouseState.LeftButton == ButtonState.Pressed &&
                       _instance._previousMouseState.LeftButton == ButtonState.Released;
            case MouseButtons.MiddleButton:
                return _instance._currentMouseState.MiddleButton == ButtonState.Pressed &&
                       _instance._previousMouseState.MiddleButton == ButtonState.Released;
            case MouseButtons.RightButton:
                return _instance._currentMouseState.RightButton == ButtonState.Pressed &&
                       _instance._previousMouseState.RightButton == ButtonState.Released;
            // Add cases for other buttons if needed
            default:
                return false;
        }
    }

    /// <summary>
    /// Returns value between -1 and 1, based on the mouse's movement to the left or right. Returns 0 when mouse is stationary
    /// </summary>
    /// <param name="mouseDelta">The type of mouse axis input</param>
    /// <returns></returns>
    public static int GetMouseAxis(MouseAxis mouseDelta)
    {
        switch (mouseDelta)
        {
            //Clamp the value of mouse axis between -1 to 1
            case MouseAxis.Horizontal:
                return Math.Clamp(_instance._currentMouseState.X - _instance._previousMouseState.X, -1, 1);
            case MouseAxis.Vertical:
                return Math.Clamp(_instance._currentMouseState.Y - _instance._previousMouseState.Y, -1, 1);
            default:
                return 0;
        }
    }

    /// <summary>
    /// Returns value from 0 to the screen viewport's max width or height. For example, if
    /// viewport width is 720px, the value mouse horizontal position will vary from 0 to 720
    /// </summary>
    /// <param name="mouseDelta"></param>
    /// <returns></returns>
    public static int GetMouseScreenPosition(MouseAxis mouseDelta)
    {
        switch (mouseDelta)
        {
            case MouseAxis.Horizontal:
                return _instance._currentMouseState.Position.X;
            case MouseAxis.Vertical:
                return _instance._currentMouseState.Position.Y;
            default:
                return 0;
        }
    }
}