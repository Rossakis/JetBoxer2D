using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.InputEvents.Base;
using Super_Duper_Shooter.Classes.InputEvents.InputTypes;

namespace Super_Duper_Shooter.Classes.InputEvents.InputMaps;

public class SplashInputMap : BaseInputMap
{
    public enum SplashInputActions
    {
        Click,
        MoveUp,
        MoveDown
    }

    private Dictionary<SplashInputActions, BaseInput> _inputMap;

    //Input Buttons
    private ButtonInput shootButton;

    //Input Axis
    private AxisInput moveHorAxis;

    public SplashInputMap()
    {
        _inputMap = new Dictionary<SplashInputActions, BaseInput>();
        InitializeButtons();
    }

    //Default button settings
    private void InitializeButtons()
    {
        moveHorAxis = new AxisInput();
    }

    //Update all input states
    public void UpdateInput(KeyboardState keyboardState)
    {
        //moveHorAxis.UpdateKeyboardValue(keyboardState.IsKeyDown(Keys.A), keyboardState.IsKeyDown(Keys.D));
        Console.WriteLine($"Keyboard Axis: {moveHorAxis.Value}");
    }

    public void AssignInput(BaseInput newInput, SplashInputActions inputAction)
    {
        _inputMap.TryAdd(inputAction, newInput);
    }

    public BaseInput GetInput(SplashInputActions inputKey)
    {
        //MoveAxis = InputActionMap.GetInput(MoveHorizontal);
        return _inputMap.GetValueOrDefault(inputKey);
    }
}