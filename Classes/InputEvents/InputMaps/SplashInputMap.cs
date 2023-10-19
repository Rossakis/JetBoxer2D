using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.InputEvents.Base;
using Super_Duper_Shooter.Classes.InputEvents.InputTypes;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.InputEvents.InputMaps;

public class EnterGame : BaseInputAction
{
    public ButtonInput InputAction { get; private set; }

    public EnterGame(ButtonInput inputAction)
    {
        InputAction = inputAction;
    }
    public static readonly Dictionary<InputDevices, List<Enum>> EnterGameDict = new();
}

public class ExitGame : BaseInputAction
{
    public ButtonInput InputAction { get; private set; }

    public ExitGame(ButtonInput inputAction)
    {
        InputAction = inputAction;
    }
    public static readonly Dictionary<InputDevices, List<Enum>> ExitGameDict = new();
}

public class SplashInputMap : BaseInputMap
{
    public static EnterGame EnterGame;
    public static ExitGame ExitGame;
    
        public SplashInputMap()
        {
            InitializeInputActions();
            InitializeInputActionDictionaries();
        }

        //Assign the default Input keys/buttons
        private void InitializeInputActions()
        {
            EnterGame = new EnterGame(new ButtonInput());
            ExitGame = new ExitGame(new ButtonInput());
        }
        
        private void InitializeInputActionDictionaries()
        {
            //Enter
            InitializeInputActionDictionary(EnterGame.EnterGameDict, InputDevices.Keyboard, new List<Enum>{Keys.Enter, Keys.Space});
            InitializeInputActionDictionary(EnterGame.EnterGameDict, InputDevices.Gamepad, new List<Enum>{Buttons.Start, Buttons.A});
            
            //Exit
            InitializeInputActionDictionary(ExitGame.ExitGameDict, InputDevices.Keyboard, new List<Enum>{Keys.Escape});
            InitializeInputActionDictionary(ExitGame.ExitGameDict, InputDevices.Gamepad, new List<Enum>{Buttons.B});
        }

        private List<Enum> GetEnterGameInput(InputDevices inputType)
        {
            // Check if the key exists in the dictionary
            if (EnterGame.EnterGameDict.TryGetValue(inputType, out var value))
                return value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"{inputType} not found in EnterGameDict dictionary");
        }

        private List<Enum> GetExitGameInput(InputDevices inputType)
        {
            if (ExitGame.ExitGameDict.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"{inputType} not found in ExitGameDict dictionary");
        }
        
        protected override void UpdateKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();

            //EnterGame
            foreach (Keys enterInput in GetEnterGameInput(InputDevices.Keyboard))
            {
                if (keyboardState.IsKeyDown(enterInput))
                {
                    EnterGame.InputAction.UpdateKeyboardValue(true);
                    break;
                }

                EnterGame.InputAction.UpdateKeyboardValue(false);
            }

            //Exit Game
            foreach (Keys exitInput in GetExitGameInput(InputDevices.Keyboard))
            {
                if (keyboardState.IsKeyDown(exitInput))
                {
                    ExitGame.InputAction.UpdateKeyboardValue(true);
                    break;
                }

                ExitGame.InputAction.UpdateKeyboardValue(false);
            }
        }

        protected override void UpdateGamepadInput()
        {
            var gamepadState = GamePad.GetState(0);

            //EnterGame - Left
            foreach (Buttons enterInput in GetEnterGameInput(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(enterInput))
                {
                    EnterGame.InputAction.UpdateGamepadValue(true); 
                    break;
                }
                EnterGame.InputAction.UpdateGamepadValue(false); 
            }
            
            //Exit Game
            foreach (Buttons exitInput in GetExitGameInput(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(exitInput))
                {
                    ExitGame.InputAction.UpdateGamepadValue(true); 
                    break;
                }
                ExitGame.InputAction.UpdateGamepadValue(false); 
            }
            
        }
    public override void RemapInputAction(Dictionary<InputDevices, List<Enum>> inputActionDict, InputDevices inputType, List<Enum> newInput)
    {
        //E.g. GameplayInputMap.RemapMoveLeft(InputTypes.Keyboard, Keys.Left)
        inputActionDict.Remove(inputType);
        inputActionDict.TryAdd(inputType, newInput);
    }
}