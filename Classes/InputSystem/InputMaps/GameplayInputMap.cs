using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.InputSystem.Base;
using Super_Duper_Shooter.Classes.InputSystem.InputTypes;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.InputSystem.InputMaps
{
    //Input Actions
    public class  MoveLeft : BaseInputAction { }
    public class  MoveRight : BaseInputAction { }
    public class MoveUp : BaseInputAction { }
    public class MoveDown : BaseInputAction { }

    public class ShootLeft : BaseInputAction { }
    public class ShootRight : BaseInputAction { }
    
    public class GameplayInputMap : BaseInputMap
    {
        public static MoveLeft MoveLeft;
        public static MoveRight MoveRight;
        public static MoveUp MoveUp;
        public static MoveDown MoveDown;
        public static ShootLeft ShootLeft;
        public static ShootRight ShootRight;

        public GameplayInputMap()
        {
            InitializeInputActions();
            InitializeInputActionDictionaries();
        }

        private void InitializeInputActions()
        {
            MoveLeft = new MoveLeft();
            MoveRight = new MoveRight();
            MoveDown = new MoveDown();
            MoveUp = new MoveUp();
            ShootLeft = new ShootLeft();
            ShootRight = new ShootRight();
        }
        
        private void InitializeInputActionDictionaries()
        {
            #region MoveHorizontal
            
            //MoveRight - First Dictionary
            MoveRight.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
            InitializeInputActionDictionary(MoveRight.ValueInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.D);
            InitializeInputActionDictionary(MoveRight.ValueInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.LeftThumbstickRight);
            
            //Second Dictionary
            MoveRight.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
            InitializeInputActionDictionary(MoveRight.ValueInputDictionaries[1].Item1, InputDevices.Keyboard, Keys.Right);
            InitializeInputActionDictionary(MoveRight.ValueInputDictionaries[1].Item1, InputDevices.Gamepad, Buttons.DPadRight);
            
            //MoveLeft - First Dictionary
            MoveLeft.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
            InitializeInputActionDictionary(MoveLeft.ValueInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.A);
            InitializeInputActionDictionary(MoveLeft.ValueInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.LeftThumbstickLeft);
            
            //Second Dictionary
            MoveLeft.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
            InitializeInputActionDictionary(MoveLeft.ValueInputDictionaries[1].Item1, InputDevices.Keyboard, Keys.Left);
            InitializeInputActionDictionary(MoveLeft.ValueInputDictionaries[1].Item1, InputDevices.Gamepad, Buttons.DPadLeft);
            #endregion
            
            #region MoveVertical
             //Move Down - First Dictionary
             MoveDown.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());// MoveVertical(X axis = Down,Y axis = Up)
             InitializeInputActionDictionary(MoveDown.ValueInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.S);
             InitializeInputActionDictionary(MoveDown.ValueInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.LeftThumbstickDown);
             
             //Second Dictionary
             MoveDown.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
             InitializeInputActionDictionary(MoveDown.ValueInputDictionaries[1].Item1, InputDevices.Keyboard, Keys.Down);
             InitializeInputActionDictionary(MoveDown.ValueInputDictionaries[1].Item1, InputDevices.Gamepad, Buttons.DPadDown);
            
             //MoveUp - First Dictionary
             MoveUp.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
             InitializeInputActionDictionary(MoveUp.ValueInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.W);
             InitializeInputActionDictionary(MoveUp.ValueInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.LeftThumbstickUp);
             
             //Second Dictionary
             MoveUp.AddValueToInputDictionary(new Dictionary<InputDevices, Enum>(), new ValueInput());
             InitializeInputActionDictionary(MoveUp.ValueInputDictionaries[1].Item1, InputDevices.Keyboard, Keys.Up);
             InitializeInputActionDictionary(MoveUp.ValueInputDictionaries[1].Item1, InputDevices.Gamepad, Buttons.DPadUp);
             #endregion

            #region Shoot Left
            ShootLeft.AddButtonToInputDictionary(new Dictionary<InputDevices, Enum>(), new ButtonInput());//Initialize Dictionary along with the InputType
            InitializeInputActionDictionary(ShootLeft.ButtonInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.J);
            InitializeInputActionDictionary(ShootLeft.ButtonInputDictionaries[0].Item1, InputDevices.Mouse, MouseInputTypes.LeftButton);
            InitializeInputActionDictionary(ShootLeft.ButtonInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.LeftTrigger);
            #endregion
            
            #region Shoot Right
            ShootRight.AddButtonToInputDictionary(new Dictionary<InputDevices, Enum>(), new ButtonInput());//Initialize Dictionary along with the InputType
            InitializeInputActionDictionary(ShootRight.ButtonInputDictionaries[0].Item1, InputDevices.Keyboard, Keys.K);
            InitializeInputActionDictionary(ShootRight.ButtonInputDictionaries[0].Item1, InputDevices.Mouse, MouseInputTypes.RightButton);
            InitializeInputActionDictionary(ShootRight.ButtonInputDictionaries[0].Item1, InputDevices.Gamepad, Buttons.RightTrigger);
            #endregion
        }

        #region Input Dictionary Finders
        private Keys GetInputFromKeyboard(Dictionary<InputDevices, Enum> inputDict)
        {
            // Check if the key exists in the dictionary
            if (inputDict.TryGetValue(InputDevices.Keyboard, out var value))
                return (Keys)value; // Return the value associated with the key

            return Keys.None;
        }
        
        private MouseInputTypes GetInputFromMouse(Dictionary<InputDevices, Enum> inputDict)
        {
            if (inputDict.TryGetValue(InputDevices.Mouse, out var value))
                return (MouseInputTypes) value;
            
            return MouseInputTypes.None;
        }
        
        private Buttons GetInputFromGamepad(Dictionary<InputDevices, Enum> inputDict)
        {
            if (inputDict.TryGetValue(InputDevices.Gamepad, out var value))
                return (Buttons)value;
            
            return Buttons.None;
        }
        #endregion

        #region Input Updates
        protected override void UpdateKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();
            
            //MoveLeft
            foreach (var dictionaryPair in MoveLeft.ValueInputDictionaries)// -1 = Left, 1 = Right
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(-1);//-1 for left
                    break;
                }
            
                dictionaryPair.Item2.UpdateKeyboardValue(0);
            }
            
            //MoveRight
            foreach (var dictionaryPair in MoveRight.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(1);//1 for right
                    break;
                }
            
                dictionaryPair.Item2.UpdateKeyboardValue(0);
            }
            
            //MoveDown
            foreach (var dictionaryPair in MoveDown.ValueInputDictionaries)// -1 = Down, 1 = Up
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(-1);//1 for down
                    break;
                }
            
                dictionaryPair.Item2.UpdateKeyboardValue(0);
            }
            
            //MoveUp
            foreach (var dictionaryPair in MoveUp.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(1);//1 for up
                    break;
                }
            
                dictionaryPair.Item2.UpdateKeyboardValue(0);
            }
            
            //Shoot Left
            foreach (var dictionaryPair in ShootLeft.ButtonInputDictionaries)// MoveHorizontal(X axis = Left, Y axis = Right)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(true);
                    break;
                }
            
                dictionaryPair.Item2.UpdateKeyboardValue(false);
            }
            
            //Shoot Right
            foreach (var dictionaryPair in ShootRight.ButtonInputDictionaries)// MoveHorizontal(X axis = Left, Y axis = Right)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (keyboardState.IsKeyDown(GetInputFromKeyboard(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateKeyboardValue(true);
                    break;
                }
            
                dictionaryPair.Item2.UpdateKeyboardValue(false);
            }
            
            //Shoot Left
            foreach (var dictionaryPair in ShootLeft.ButtonInputDictionaries)
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
            //MoveLeft
            foreach (var dictionaryPair in MoveLeft.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (MouseInput.GetMouseAxis(GetInputFromMouse(inputDictionary)) < 0)//if mouse axis horizontal is -1
                {
                    dictionaryPair.Item2.UpdateMouseValue(-1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateMouseValue(0);
            }
            
            //MoveRight
            foreach (var dictionaryPair in MoveRight.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (MouseInput.GetMouseAxis(GetInputFromMouse(inputDictionary)) > 0)//if mouse axis horizontal is 1
                {
                    dictionaryPair.Item2.UpdateMouseValue(1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateMouseValue(0);
            }
            
            //MoveDown
            foreach (var dictionaryPair in MoveDown.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (MouseInput.GetMouseAxis(GetInputFromMouse(inputDictionary)) < 0)
                {
                    dictionaryPair.Item2.UpdateMouseValue(-1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateMouseValue(0);
            }
            
            //MoveUp
            foreach (var dictionaryPair in MoveUp.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (MouseInput.GetMouseAxis(GetInputFromMouse(inputDictionary)) > 0)
                {
                    dictionaryPair.Item2.UpdateMouseValue(1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateMouseValue(0);
            }
            
            //Shoot Left
            foreach (var dictionaryPair in ShootLeft.ButtonInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (MouseInput.IsButtonDown(GetInputFromMouse(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateMouseValue(true);
                    break;
                }
            
                dictionaryPair.Item2.UpdateMouseValue(false);
            }
            
            //Shoot Right
            foreach (var dictionaryPair in ShootRight.ButtonInputDictionaries)
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
            
             //MoveLeft
            foreach (var dictionaryPair in MoveLeft.ValueInputDictionaries)// -1 = Left, 1 = Right
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(-1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateGamepadValue(0);
            }
            
            //MoveRight
            foreach (var dictionaryPair in MoveRight.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateGamepadValue(0);
            }
            
            //MoveDown
            foreach (var dictionaryPair in MoveDown.ValueInputDictionaries)// -1 = Down, 1 = Up
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(-1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateGamepadValue(0);
            }
            
            //MoveUp
            foreach (var dictionaryPair in MoveUp.ValueInputDictionaries)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(1);
                    break;
                }
            
                dictionaryPair.Item2.UpdateGamepadValue(0);
            }
            
            //Shoot Left
            foreach (var dictionaryPair in ShootLeft.ButtonInputDictionaries)// MoveHorizontal(X axis = Left, Y axis = Right)
            {
                var inputDictionary = dictionaryPair.Item1;
                if (gamepadState.IsButtonDown(GetInputFromGamepad(inputDictionary)))
                {
                    dictionaryPair.Item2.UpdateGamepadValue(true);
                    break;
                }
            
                dictionaryPair.Item2.UpdateGamepadValue(false);
            }
            
            //Shoot Right
            foreach (var dictionaryPair in ShootRight.ButtonInputDictionaries)// MoveHorizontal(X axis = Left, Y axis = Right)
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

        #endregion
        
        /// <summary>
        /// Remap the an Input Action Dictionary key's value
        /// </summary>
        public override void RemapInputAction(Dictionary<InputDevices, List<Enum>> inputActionDict, InputDevices inputType, List<Enum> newInput)
        {
            //E.g. RemapInputAction(MoveHorizontal.MoveHorLeftDict, InputTypes.Keyboard, Keys.Left)
            inputActionDict.Remove(inputType);
            inputActionDict.TryAdd(inputType, newInput);
        }
    }
}