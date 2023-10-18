using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Classes.InputEvents.Base;
using Super_Duper_Shooter.Classes.InputEvents.InputMaps;
using Super_Duper_Shooter.Classes.InputEvents.InputTypes;

namespace Super_Duper_Shooter.Classes.InputEvents;

public class InputManager
{
    private enum ActiveInput
    {
        KeyboardMouse,
        Gamepad
    }

    // public GameplayInputMap InputActionMap => _gameplayInputMap;
    // public SplashInputMap SplashInputMap => _splashInputMap;

    private ActiveInput _currentActiveInput;
    private List<ValueType> _currentInputStates; //store KeyboardState, MouseState adn GamepadState here
    private List<ValueType> _previousInputStates;

    //Using these three states, we  will be able to learn the differences
    //between _currentKeyboardState and _previousKeyboardState, to create
    //the PressedDown and ReleasedUp methods

    private GameplayInputMap _gameplayInputMap;
    private SplashInputMap _splashInputMap;
    
    private ButtonInput _previousButtonState;//for GetButtonDown

    private static InputManager _instance;
    
    public static InputManager Instance
    {
        get //if instance = null, create new
        {
            if (_instance == null)
                throw new Exception("InputManager.Instance wasn't instantiated");
    
            return _instance;
        }
        set => _instance = value;
    }

    public InputManager()
    {
        _gameplayInputMap = new GameplayInputMap();
        _splashInputMap = new SplashInputMap();
    }

    //Update all the input states
    
    public void UpdateGameplayInput()
    {
        _gameplayInputMap.UpdateInput();
    }

    public void UpdateSplashInput()
    {
        //_splashInputMap.UpdateInput();
    }

    public void GetButton(ButtonInput button)
    {
    }

    public bool GetButtonDown(ButtonInput button)
    {
        if (!button.Pressed && _previousButtonState.Pressed)
        {
            _previousButtonState = button;
            return true;
        }
        _previousButtonState = button;
        return false;
    }

    public static void GetButtonUp(ButtonInput button)
    {
    }

    public static Vector2 GetAxis(AxisInput axis)
    {
        return axis.Value;
    }

    // See which input is currently active (gamepad has priority)
    // public object GetActiveInput(BaseInput input)
    // {
    // }
}