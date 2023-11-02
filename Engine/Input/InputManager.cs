using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Engine.Input.Enums;
using Super_Duper_Shooter.Engine.Input.InputTypes;
using Super_Duper_Shooter.Engine.Input.Objects;

namespace Super_Duper_Shooter.Engine.Input;

public class InputManager
{
    private BaseInputMap _currentInputMap;
    
    public InputManager(BaseInputMap inputMap)
    {
        //Default Input Map
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
        if (inputAction.ButtonInputDictionaries == null)
            throw new ArgumentException($"The Button Dictionary of {inputAction} is empty");
        
        foreach (var list in inputAction.ButtonInputDictionaries)
        {
            //Item2 is ButtonInput in this instance
            if(list.Item2.Pressed)
                return list.Item2.Pressed;
        }

        return false;
    }

    public bool GetButtonDown(BaseInputAction inputAction)
    {
        //return inputAction.ButtonInputDictionaries[0].Item2.PressedDown ||  inputAction.ButtonInputDictionaries[1].Item2.PressedDown;
        if (inputAction.ButtonInputDictionaries == null)
            throw new ArgumentException($"The Button Dictionary of {nameof(inputAction)} is empty");
        
        foreach (var list in inputAction.ButtonInputDictionaries)
        {
            //Item2 is ButtonInput in this instance
            if (list.Item2.PressedDown)
                return true;
        }
        
        return false;
    }

    public Vector2 GetAxisValue(BaseInputAction inputAction)
    {
        if (inputAction.AxisInputDictionaries == null)
            throw new ArgumentException($"The Axis Dictionary of {inputAction} is empty");
        
        foreach (var list in inputAction.AxisInputDictionaries)
        {
            //Item2 is ButtonInput in this instance
            if(list.Item2.Value != Vector2.Zero)
                return list.Item2.Value;
        }

        return Vector2.Zero;
    }

    public int GetValue(BaseInputAction inputAction)
    {
        if (inputAction.ValueInputDictionaries == null)
            throw new ArgumentException($"The Value Dictionary of {inputAction} is empty");
        
        foreach (var list in inputAction.ValueInputDictionaries)
        {
            //Item2 is ButtonInput in this instance
            if(list.Item2.Value != 0)
                return list.Item2.Value;
        }
        
        return 0;
    }

    public  void RemapInput(Dictionary<InputDevices,List<Enum>> inputActionDict, InputDevices inputDevice, List<Enum> newButtons)
    {
       _currentInputMap.RemapInputAction(inputActionDict, inputDevice, newButtons);
    }
}