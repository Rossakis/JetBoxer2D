using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JetBoxer2D.Engine.Extensions;

/// <summary>
/// A wrapper class for Mouse Input, so that we input information from it in the same way we do for Keyboard and Gamepad
///classes (e.g. Mouse.IsButtonDown(MouseButton.RightButton)
/// </summary>
public enum MouseInputTypes
{
    None,
    RightButton,
    MiddleButton,
    LeftButton,
    Horizontal,
    Vertical
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
                return _instance = new MouseInput();

            return _instance;
        }
        set => _instance = value;
    }

    public static void Update()
    {
        _instance._previousMouseState = _instance._currentMouseState;
        _instance._currentMouseState = Mouse.GetState();
    }

    public static bool IsButtonDown(MouseInputTypes button)
    {
        return button switch
        {
            MouseInputTypes.LeftButton => _instance._currentMouseState.LeftButton == ButtonState.Pressed,
            MouseInputTypes.MiddleButton => _instance._currentMouseState.MiddleButton == ButtonState.Pressed,
            MouseInputTypes.RightButton => _instance._currentMouseState.RightButton == ButtonState.Pressed,
            _ => false
        };
    }

    public static bool IsButtonUp(MouseInputTypes button)
    {
        return button switch
        {
            MouseInputTypes.LeftButton => _instance._currentMouseState.LeftButton == ButtonState.Released &&
                                          _instance._previousMouseState.LeftButton == ButtonState.Pressed,
            MouseInputTypes.MiddleButton => _instance._currentMouseState.MiddleButton == ButtonState.Released &&
                                            _instance._previousMouseState.MiddleButton == ButtonState.Pressed,
            MouseInputTypes.RightButton => _instance._currentMouseState.RightButton == ButtonState.Released &&
                                           _instance._previousMouseState.RightButton == ButtonState.Pressed,
            _ => false
        };
    }

    /// <summary>
    /// Was the button pressed for at least one frame
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetButtonDown(MouseInputTypes button)
    {
        return button switch
        {
            MouseInputTypes.LeftButton => _instance._currentMouseState.LeftButton == ButtonState.Pressed &&
                                          _instance._previousMouseState.LeftButton == ButtonState.Released,
            MouseInputTypes.MiddleButton => _instance._currentMouseState.MiddleButton == ButtonState.Pressed &&
                                            _instance._previousMouseState.MiddleButton == ButtonState.Released,
            MouseInputTypes.RightButton => _instance._currentMouseState.RightButton == ButtonState.Pressed &&
                                           _instance._previousMouseState.RightButton == ButtonState.Released,
            _ => false
        };
    }

    /// <summary>
    /// Returns value between -1 and 1, based on the mouse's Horizontal or Vertical movement. Returns 0 when mouse is stationary
    /// </summary>
    /// <param name="mouseDelta">The type of mouse axis input</param>
    /// <returns></returns>
    public static float GetMouseAxis(MouseInputTypes mouseDelta)
    {
        switch (mouseDelta)
        {
            case MouseInputTypes.Horizontal:
                return _instance._currentMouseState.X -
                       _instance._previousMouseState.X;
            case MouseInputTypes.Vertical:
                return -(_instance._currentMouseState.Y -
                         _instance._previousMouseState.Y);
            case MouseInputTypes.None:
                break;
        }

        return 0;
    }

    /// <summary>
    /// Returns the mouse's position on the screen viewport.
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetMouseScreenPosition()
    {
        return _instance._currentMouseState.Position.ToVector2();
    }

    /// <summary>
    /// Returns value from 0 to the screen viewport's max width or height. For example, if
    /// viewport width is 720px, the value mouse horizontal position will vary from 0 to 720
    /// </summary>
    /// <param name="mouseDelta"></param>
    /// <returns></returns>
    public static int GetMouseScreenPosition(MouseInputTypes mouseDelta)
    {
        return mouseDelta switch
        {
            MouseInputTypes.Horizontal => _instance._currentMouseState.Position.X,
            MouseInputTypes.Vertical => _instance._currentMouseState.Position.Y,
            _ => 0
        };
    }
}