using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Engine.Extensions;
using Super_Duper_Shooter.Engine.Input.Enums;
using Super_Duper_Shooter.Engine.Input.InputTypes;
using Super_Duper_Shooter.Engine.Input.Objects;

namespace Super_Duper_Shooter.Game.InputMaps;

public class EnterGame : BaseInputAction { }
public class ExitGame : BaseInputAction { }

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
            EnterGame = new EnterGame();
            ExitGame = new ExitGame();
        }
        
        private void InitializeInputActionDictionaries()
        {
            //EnterGame - First Dictionary
            EnterGame.AddButtonToInputDictionary(new Dictionary<InputDevices, Enum>(), new ButtonInput());//Initialize Dictionary along with the InputType
            InitializeInputActionDictionary(EnterGame.ButtonInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.Space);
            InitializeInputActionDictionary(EnterGame.ButtonInputDictionaries[0].Item1, InputDevices.Mouse, MouseInputTypes.LeftButton);
            InitializeInputActionDictionary(EnterGame.ButtonInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.Start);

            //EnterGame - Second Dictionary
            EnterGame.AddButtonToInputDictionary(new Dictionary<InputDevices, Enum>(), new ButtonInput());
            InitializeInputActionDictionary(EnterGame.ButtonInputDictionaries[1].Item1, InputDevices.Keyboard, Keys.Enter);
            InitializeInputActionDictionary(EnterGame.ButtonInputDictionaries[1].Item1, InputDevices.Mouse, MouseInputTypes.RightButton);
            InitializeInputActionDictionary(EnterGame.ButtonInputDictionaries[1].Item1, InputDevices.Gamepad, Buttons.A);
            
            //ExitGame
            ExitGame.AddButtonToInputDictionary(new Dictionary<InputDevices, Enum>(), new ButtonInput());
            InitializeInputActionDictionary(ExitGame.ButtonInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.Escape);//First Dictionary
            InitializeInputActionDictionary(ExitGame.ButtonInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.Back);
        }

        #region  Input Retrieval
        private Keys GetInputFromKeyboard(Dictionary<InputDevices, Enum> inputDict)
        {
            // Check if the key exists in the dictionary
            if (inputDict.TryGetValue(InputDevices.Keyboard, out var value))
                return (Keys)value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"{InputDevices.Keyboard} not found in {inputDict} dictionary");
        }
        
        private MouseInputTypes GetInputFromMouse(Dictionary<InputDevices, Enum> inputDict)
        {
            // Check if the key exists in the dictionary
            if (inputDict.TryGetValue(InputDevices.Mouse, out var value))
                return (MouseInputTypes)value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"{InputDevices.Mouse} not found in {inputDict} dictionary");
        }
        
        private Buttons GetInputFromGamepad(Dictionary<InputDevices, Enum> inputDict)
        {
            // Check if the key exists in the dictionary
            if (inputDict.TryGetValue(InputDevices.Gamepad, out var value))
                return (Buttons)value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"{InputDevices.Gamepad} not found in {inputDict} dictionary");
        }
        #endregion
        
        
        protected override void UpdateKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();
            
            //Enter Game - Button Dictionaries loop
            foreach (var dictionaryPair in EnterGame.ButtonInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(true);
                    break;
                }

                dictionaryPair.Item2.UpdateKeyboardValue(false);
            }

            //Exit Game
            foreach (var dictionaryPair in ExitGame.ButtonInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(true);
                    break;
                }

                dictionaryPair.Item2.UpdateKeyboardValue(false);
            }
        }

        protected override void UpdateMouseInput()
        {
            //EnterGame
            foreach (var dictionaryPair in EnterGame.ButtonInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (MouseInput.IsButtonDown(GetInputFromMouse(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateMouseValue(true);
                    break;
                }

                dictionaryPair.Item2.UpdateMouseValue(false);
            }
        }

        protected override void UpdateGamepadInput()
        {
            var gamepadState = GamePad.GetState(0);

            //EnterGame
            foreach (var dictionaryPair in EnterGame.ButtonInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(true);
                    break;
                }

                dictionaryPair.Item2.UpdateGamepadValue(false);
            }
            
            //Exit Game
            foreach (var dictionaryPair in ExitGame.ButtonInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(true);
                    break;
                }

                dictionaryPair.Item2.UpdateGamepadValue(false);
            }
            
        }
    public override void RemapInputAction(Dictionary<InputDevices, List<Enum>> inputActionDict, InputDevices inputType, List<Enum> newInput)
    {
        inputActionDict.Remove(inputType);
        inputActionDict.TryAdd(inputType, newInput);
    }
}