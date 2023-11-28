using System;
using System.Collections.Generic;
using JetBoxer2D.Engine.Input.Enums;
using JetBoxer2D.Engine.Input.Objects;
using JetBoxer2D.Game.InputMaps;
using JetBoxer2D.Game.States;
using Microsoft.Xna.Framework;

namespace JetBoxer2D.Engine.Input;

public class InputManager
{
    private BaseInputMap _currentInputMap;

    public SplashInputMap _splashInputMap;
    private GameplayInputMap _gameplayInputMap;

    public InputManager(BaseInputMap inputMap)
    {
        //Default Input Map
        _splashInputMap = new SplashInputMap();
        _gameplayInputMap = new GameplayInputMap();

        _currentInputMap = inputMap;
    }

    public void SetInputMap(BaseInputMap inputMap)
    {
        _currentInputMap = inputMap;
    }

    public void UpdateInput()
    {
        _currentInputMap.UpdateInput();
    }

    public bool GetButton(BaseInputAction inputAction)
    {
        if (inputAction == null)
            throw new ArgumentException($"A BaseInputAction in this InputMap hasn't been initialized");

        if (inputAction.ButtonInputDictionaries == null)
            throw new ArgumentException($"The Button Dictionary of {inputAction} is empty");

        foreach (var list in inputAction.ButtonInputDictionaries)
            //Item2 is ButtonInput in this instance
            if (list.Item2.Pressed)
                return list.Item2.Pressed;

        return false;
    }

    public bool GetButtonDown(BaseInputAction inputAction)
    {
        if (inputAction == null)
            throw new ArgumentException($"A BaseInputAction in this InputMap hasn't been initialized");

        if (inputAction.ButtonInputDictionaries == null)
            throw new ArgumentException($"The Button Dictionary of {nameof(inputAction)} is empty");

        foreach (var list in inputAction.ButtonInputDictionaries)
            //Item2 is ButtonInput in this instance
            if (list.Item2.PressedDown)
                return true;

        return false;
    }

    public Vector2 GetAxisValue(BaseInputAction inputAction)
    {
        if (inputAction == null)
            throw new ArgumentException($"A BaseInputAction in this InputMap hasn't been initialized");

        if (inputAction.AxisInputDictionaries == null)
            throw new ArgumentException($"The Axis Dictionary of {inputAction} is empty");

        foreach (var list in inputAction.AxisInputDictionaries)
            //Item2 is ButtonInput in this instance
            if (list.Item2.Value != Vector2.Zero)
                return list.Item2.Value;

        return Vector2.Zero;
    }

    public float GetValue(BaseInputAction inputAction)
    {
        if (inputAction == null)
            throw new ArgumentException($"A BaseInputAction in this InputMap hasn't been initialized");

        if (inputAction.ValueInputDictionaries == null)
            throw new ArgumentException($"The Value Dictionary of {inputAction} is empty");

        foreach (var list in inputAction.ValueInputDictionaries)
            //Item2 is ButtonInput in this instance
            if (list.Item2.Value != 0)
                return list.Item2.Value;

        return 0;
    }

    public void RemapInput(Dictionary<InputDevices, List<Enum>> inputActionDict, InputDevices inputDevice,
        List<Enum> newButtons)
    {
        _currentInputMap.RemapInputAction(inputActionDict, inputDevice, newButtons);
    }
}