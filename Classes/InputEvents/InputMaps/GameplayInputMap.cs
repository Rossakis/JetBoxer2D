using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.InputEvents.Base;
using Super_Duper_Shooter.Classes.InputEvents.InputTypes;

namespace Super_Duper_Shooter.Classes.InputEvents.InputMaps;

public class GameplayInputMap : BaseInputMap
{
    //Accessed by BaseInput classes
    private static class GameplayInputActions
    {
        //Make InputActions dictionaries agnostic to the input value (e.g. Keys, Buttons, etc...)
        //E.g. List<Input, typeOfInput> moveVertical = { (Keys.A, Keyboard), (Buttons.LeftStick, Gamepad) } 

        public static readonly Dictionary<Enums.InputTypes, object> MoveHorizontalLeft = new(); //Axis
        public static readonly Dictionary<Enums.InputTypes, object> MoveHorizontalRight = new();

        public static readonly Dictionary<Enums.InputTypes, object> ShootLeft = new(); //Axis
        public static readonly Dictionary<Enums.InputTypes, object> ShootRight = new();
    }

    //Input Axis
    private static AxisInput _moveHorAxis;
    private static AxisInput _shootAxis; //-1 for left shooting, 1 for right shooting

    public GameplayInputMap()
    {
        AssignInputActions();
        InitializeInput();
    }

    //Assign the default Input keys/buttons
    private void AssignInputActions()
    {
        //MoveHorizontal
        GameplayInputActions.MoveHorizontalLeft.TryAdd(Enums.InputTypes.Keyboard, Keys.A); //Keyboard
        GameplayInputActions.MoveHorizontalRight.TryAdd(Enums.InputTypes.Keyboard, Keys.D);

        GameplayInputActions.MoveHorizontalLeft.TryAdd(Enums.InputTypes.Gamepad, Buttons.LeftThumbstickLeft); //Gamepad
        GameplayInputActions.MoveHorizontalRight.TryAdd(Enums.InputTypes.Gamepad, Buttons.LeftThumbstickRight);

        GameplayInputActions.MoveHorizontalRight.TryAdd(Enums.InputTypes.Mouse, MouseAxis.Horizontal); //Mouse
        GameplayInputActions.MoveHorizontalRight.TryAdd(Enums.InputTypes.Mouse, Buttons.LeftThumbstickRight);


        //Shoot
        GameplayInputActions.ShootLeft.TryAdd(Enums.InputTypes.Keyboard, Keys.J); //Keyboard
        GameplayInputActions.ShootRight.TryAdd(Enums.InputTypes.Keyboard, Keys.K);

        GameplayInputActions.ShootLeft.TryAdd(Enums.InputTypes.Gamepad, Buttons.LeftTrigger); //Gamepad
        GameplayInputActions.ShootRight.TryAdd(Enums.InputTypes.Gamepad, Buttons.RightTrigger);

        GameplayInputActions.ShootLeft.TryAdd(Enums.InputTypes.Mouse, MouseButtons.LeftButton); //Mouse
        GameplayInputActions.ShootRight.TryAdd(Enums.InputTypes.Mouse, MouseButtons.RightButton);
    }

    private void InitializeInput()
    {
        _moveHorAxis = new AxisInput();
        _shootAxis = new AxisInput();
    }

    #region Input Dictionary Finders

    private static T InputMoveLeft<T>(Enums.InputTypes inputType)
    {
        // Check if the key exists in the dictionary
        if (GameplayInputActions.MoveHorizontalLeft.TryGetValue(inputType, out var value))
            return (T) value; // Return the value associated with the key
        else
            throw new KeyNotFoundException($"InputTypes.{inputType} not found in MoveHorizontalLeft dictionary");
    }

    private static T InputMoveRight<T>(Enums.InputTypes inputType)
    {
        if (GameplayInputActions.MoveHorizontalRight.TryGetValue(inputType, out var value))
            return (T) value;
        else
            throw new KeyNotFoundException($"InputTypes.{inputType} not found in MoveHorizontalRight dictionary");
    }

    //The reason we make this generic, is because the ButtonState return that we need for the mouse is not 
    private static T InputShootLeft<T>(Enums.InputTypes inputType)
    {
        if (GameplayInputActions.ShootLeft.TryGetValue(inputType, out var value))
            return (T) value;
        else
            throw new KeyNotFoundException($"InputTypes.{inputType} not found in Shoot dictionary");
    }

    private static T InputShootRight<T>(Enums.InputTypes inputType)
    {
        if (GameplayInputActions.ShootRight.TryGetValue(inputType, out var value))
            return (T) value;
        else
            throw new KeyNotFoundException($"InputTypes.{inputType} not found in Shoot dictionary");
    }

    #endregion

    #region Input Updates

    //Update all input states
    public static void UpdateInput()
    {
        UpdateKeyboardInput();
        UpdateGamepadState();
        UpdateMouseInput();

        // return this;
    }

    private static void UpdateKeyboardInput()
    {
        var keyboardState = Keyboard.GetState();

        _moveHorAxis.UpdateKeyboardValue(
            keyboardState.IsKeyDown(InputMoveLeft<Keys>(Enums.InputTypes.Keyboard)),
            keyboardState.IsKeyDown(InputMoveRight<Keys>(Enums.InputTypes.Keyboard)));

        _shootAxis.UpdateGamepadValue(
            keyboardState.IsKeyDown(InputShootLeft<Keys>(Enums.InputTypes.Keyboard)),
            keyboardState.IsKeyDown(InputShootRight<Keys>(Enums.InputTypes.Keyboard)));

        //Console.WriteLine($"Mouse Button State: {_shootAxis.MouseValue}");
    }

    private static void UpdateMouseInput()
    {
        _shootAxis.UpdateGamepadValue(
            MouseInput.IsButtonPressed(InputShootLeft<MouseButtons>(Enums.InputTypes.Mouse)),
            MouseInput.IsButtonPressed(InputShootRight<MouseButtons>(Enums.InputTypes.Mouse)));

        //Console.WriteLine($"Mouse Button State: {_shootAxis.MouseValue}");
    }

    private static void UpdateGamepadState()
    {
        var gamepadState = GamePad.GetState(0);

        _moveHorAxis.UpdateGamepadValue(
            gamepadState.IsButtonDown(InputMoveLeft<Buttons>(Enums.InputTypes.Gamepad)),
            gamepadState.IsButtonDown(InputMoveRight<Buttons>(Enums.InputTypes.Gamepad)));

        _shootAxis.UpdateGamepadValue(
            gamepadState.IsButtonDown(InputShootLeft<Buttons>(Enums.InputTypes.Gamepad)),
            gamepadState.IsButtonDown(InputShootRight<Buttons>(Enums.InputTypes.Gamepad)));
    }

    #endregion

    #region Input Remapping

    /// <summary>
    /// Remap the <see cref="GameplayInputActions.MoveHorizontalLeft"/> dictionary's input action to a desired new one
    /// </summary>
    /// <param name="inputType"></param>
    /// <param name="newInput">Keys, Buttons, MouseButtons</param>
    public void RemapMoveLeft(Enums.InputTypes inputType, object newInput)
    {
        //E.g. GameplayInputMap.RemapMoveLeft(InputTypes.Keyboard, Keys.Left)

        GameplayInputActions.MoveHorizontalLeft.Remove(inputType);
        GameplayInputActions.MoveHorizontalLeft.Add(inputType, newInput);
    }

    #endregion
}