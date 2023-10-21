using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Classes.InputSystem.Base;
using Super_Duper_Shooter.Classes.InputSystem.InputMaps;
using Super_Duper_Shooter.Classes.InputSystem.InputTypes;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.InputSystem;

public class InputManager
{
    private readonly GameplayInputMap _gameplayInputMap;
    private readonly SplashInputMap _splashInputMap;
    private BaseInputMap _currentInputMap;

    private ButtonInput _previousButtonState;//for GetButtonDown

    // private static InputManager _instance;
    //
    // public static InputManager Instance
    // {
    //     get //if instance = null, create new
    //     {
    //         if (_instance == null)
    //             return _instance = new InputManager();
    //         
    //         return _instance;
    //     }
    //     set => _instance = value;
    // }

    public InputManager(BaseInputMap inputMap)
    {
        // _gameplayInputMap = new GameplayInputMap();
        // _splashInputMap = new SplashInputMap();

        //Default Input Map
        _currentInputMap = inputMap;
        _previousButtonState = new ButtonInput();
    }
    
    public void SetInputMap(BaseInputMap inputMap)
    {
        if (inputMap is GameplayInputMap)
            _currentInputMap = _gameplayInputMap;
        else if (inputMap is SplashInputMap)
            _currentInputMap = _splashInputMap;
        else
            throw new ArgumentException("Wrong Input Map assignment");
    }
    
    public void UpdateInput()
    {
        _currentInputMap.UpdateInput();
    }
    

    public bool GetButton(ButtonInput button)
    {
        return button.Pressed;
    }

    public bool GetButtonDown(ButtonInput button)
    {
        if (button.Pressed && !_previousButtonState.Pressed)
        {
            _previousButtonState = button;
            return true;
        }
        
        _previousButtonState = button;
        return false;
    }

    public void GetButtonUp(ButtonInput button)
    {
    }

    public Vector2 GetAxis(AxisInput axis)
    {
        return axis.Value;
    }

    public  void RemapInput(Dictionary<InputDevices,List<Enum>> inputActionDict, InputDevices inputDevice, List<Enum> newButtons)
    {
       _currentInputMap.RemapInputAction(inputActionDict, inputDevice, newButtons);
    }
}