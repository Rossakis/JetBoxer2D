using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.InputEvents.Base;
using Super_Duper_Shooter.Classes.InputEvents.InputTypes;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.InputEvents.InputMaps
{
    public class GameplayInputMap : BaseInputMap
    {
        //Accessed by BaseInput classes
        private static class GameplayInputActions
        {
            //Make InputActions dictionaries agnostic to the input value (e.g. Keys, Buttons, etc...)
            //For each InputAction (e.g. Move Left) exists a dictionary, where for each input device (keyboard, mouse, etc...)
            //exists an Enum list containing all the input names for each device (e.g. Keys for Keyboard, Buttons for Gamepad, etc...)

            public static readonly Dictionary<InputDevices, List<Enum>> MoveHorizontalLeft = new(); //Axis
            public static readonly Dictionary<InputDevices, List<Enum>> MoveHorizontalRight = new();

            public static readonly Dictionary<InputDevices, List<Enum>> ShootLeft = new(); //Axis
            public static readonly Dictionary<InputDevices, List<Enum>> ShootRight = new();
        }

        //Input Axis
        public static AxisInput _moveHorAxis;
        private static AxisInput _shootAxis; //-1 for left shooting, 1 for right shooting

        public GameplayInputMap()
        {
            InitializeInputActions();
            InitializeInputButtons();
        }

        //Assign the default Input keys/buttons
        private void InitializeInputActions()
        {
            //MoveHorizontal - Keyboard
            var leftKeys = new List<Enum>() {Keys.A, Keys.Left};//Keys responsible for performing the left move action
            var rightKeys = new List<Enum>() {Keys.D, Keys.Right};
            GameplayInputActions.MoveHorizontalLeft.TryAdd(InputDevices.Keyboard, leftKeys); 
            GameplayInputActions.MoveHorizontalRight.TryAdd(InputDevices.Keyboard, rightKeys);

            //MoveHorizontal - Gamepad
            var leftGpButtons = new List<Enum>() {Buttons.LeftThumbstickLeft};//Gamepad buttons responsible for performing the left move action
            var rightGpButtons = new List<Enum>() {Buttons.LeftThumbstickRight};
            GameplayInputActions.MoveHorizontalLeft.TryAdd(InputDevices.Gamepad, leftGpButtons);
            GameplayInputActions.MoveHorizontalRight.TryAdd(InputDevices.Gamepad, rightGpButtons);

            //MoveHorizontal - Mouse
            var leftMsAxis = new List<Enum>() {MouseAxis.Horizontal};//Gamepad buttons responsible for performing the left move action
            var rightMsAxis = new List<Enum>() {MouseAxis.Vertical};
            GameplayInputActions.MoveHorizontalLeft.TryAdd(InputDevices.Mouse, leftMsAxis); //Mouse
            GameplayInputActions.MoveHorizontalRight.TryAdd(InputDevices.Mouse, rightMsAxis);


            // //Shoot
            // GameplayInputActions.ShootLeft.TryAdd(InputDevices.Keyboard, Keys.J); //Keyboard
            // GameplayInputActions.ShootRight.TryAdd(InputDevices.Keyboard, Keys.K);
            //
            // GameplayInputActions.ShootLeft.TryAdd(InputDevices.Gamepad, Buttons.LeftTrigger); //Gamepad
            // GameplayInputActions.ShootRight.TryAdd(InputDevices.Gamepad, Buttons.RightTrigger);
            //
            // GameplayInputActions.ShootLeft.TryAdd(InputDevices.Mouse, MouseButtons.LeftButton); //Mouse
            // GameplayInputActions.ShootRight.TryAdd(InputDevices.Mouse, MouseButtons.RightButton);
        }

        private void InitializeInputButtons()
        {
            _moveHorAxis = new AxisInput();
            _shootAxis = new AxisInput();
        }

        #region Input Dictionary Finders

        private static List<Enum> InputMoveLeft(InputDevices inputType)
        {
            // Check if the key exists in the dictionary
            if (GameplayInputActions.MoveHorizontalLeft.TryGetValue(inputType, out var value))
                return value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"InputTypes.{inputType} not found in MoveHorizontalLeft dictionary");
        }

        private static List<Enum> InputMoveRight(InputDevices inputType)
        {
            if (GameplayInputActions.MoveHorizontalRight.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"InputTypes.{inputType} not found in MoveHorizontalRight dictionary");
        }

        //The reason we make this generic, is because the ButtonState return that we need for the mouse is not 
        private static List<Enum> InputShootLeft(InputDevices inputType)
        {
            if (GameplayInputActions.ShootLeft.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"InputTypes.{inputType} not found in Shoot dictionary");
        }
        
        private static List<Enum> InputShootRight(InputDevices inputType)
        {
            if (GameplayInputActions.ShootRight.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"InputTypes.{inputType} not found in Shoot dictionary");
        }
        #endregion

        #region Input Updates
        //Update all input states
        public void UpdateInput()
        {
            UpdateKeyboardInput();
            UpdateGamepadState();
            UpdateMouseInput();

            //Console.WriteLine($"Move Horizontal: {_moveHorAxis.Value}");
        }

        private static void UpdateKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();
            
            #region MoveHorizontal
            //Left
            foreach (Keys leftInput in InputMoveLeft(InputDevices.Keyboard))
            {
                if (keyboardState.IsKeyDown(leftInput))
                {
                    _moveHorAxis.UpdateKeyboardValueX(true, _moveHorAxis.CurrentY); 
                    break;
                }
                _moveHorAxis.UpdateKeyboardValueX(false, _moveHorAxis.CurrentY);
            }
            //Right
            foreach (Keys rightInput in InputMoveRight(InputDevices.Keyboard))
            {
                if (keyboardState.IsKeyDown(rightInput))
                {
                    _moveHorAxis.UpdateKeyboardValueX(_moveHorAxis.CurrentX, true); 
                    break;
                }
                _moveHorAxis.UpdateKeyboardValueX(_moveHorAxis.CurrentX, false); 
            }
            #endregion
            
        }

        private static void UpdateMouseInput()
        {
        }

        private static void UpdateGamepadState()
        {
            var gamepadState = GamePad.GetState(0);

            #region MoveHorizontal
            //Left
            foreach (Buttons leftInput in InputMoveLeft(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(leftInput))
                {
                    _moveHorAxis.UpdateGamepadValueX(true, _moveHorAxis.CurrentY); 
                    break;
                }
                _moveHorAxis.UpdateGamepadValueX(false, _moveHorAxis.CurrentY); 
            }
            
            //Right
            foreach (Buttons rightInput in InputMoveRight(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(rightInput))
                {
                    _moveHorAxis.UpdateGamepadValueX(_moveHorAxis.CurrentX, true); 
                    break;
                }
                _moveHorAxis.UpdateGamepadValueX(_moveHorAxis.CurrentX, false); 
            }
            #endregion
            
        }

        #endregion

        #region Input Remapping

        /// <summary>
        /// Remap the <see cref="GameplayInputActions.MoveHorizontalLeft"/> dictionary's input action to a desired new one
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="newInput">Keys, Buttons, MouseButtons</param>
        public void RemapMoveLeft(InputDevices inputType, List<Enum> newInput)
        {
            //E.g. GameplayInputMap.RemapMoveLeft(InputTypes.Keyboard, Keys.Left)

            GameplayInputActions.MoveHorizontalLeft.Remove(inputType);
            GameplayInputActions.MoveHorizontalLeft.Add(inputType, newInput);
        }

        #endregion
    }
}